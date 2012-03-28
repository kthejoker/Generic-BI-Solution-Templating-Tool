using System;
//using System.Reflection;
using Microsoft.SqlServer.SSIS.EzAPI;
using WpfApplication1.Tier;
using WpfApplication1.DataSource;
using WpfApplication1.Static;
using WpfApplication1.EzCustom;

namespace WpfApplication1.Package
{

    class PACKAGE_STAGE : PACKAGE
    {

          public PACKAGE_STAGE(DATAOBJECT d) : base(d)
        {
            this.packageType = "STAGE";

            this.Name = this.tableName("STAGE");

            this.addConnection("Commercial_STG", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("STAGE").NAME);
            this.addConnection("Commercial_PSA", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("PSA").NAME);
            this.addConnection("Commercial_RUN", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("RUN").NAME);
            this.addConnection("Commercial_MATCH", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("MATCH").NAME);
            this.addConnection("Commercial_META", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("META").NAME);
            this.addConnection("Source", this.d.ds.SERVERNAME, this.d.ds.DATABASENAME);

            this.addVariable("Audit::RowsMatched");
            this.addVariable("Audit::RowsInserted");
            this.addVariable("Audit::RowsUpdated");
            this.addVariable("Audit::RowsDeleted");

            //Clear stage table for clean full load of source data
            EzExecuteSQLTask TruncateStageTable = new EzExecuteSQLTask(this);
            TruncateStageTable.Name = "Truncate Stage Table";
            TruncateStageTable.SqlStatementSource = "truncate table " + this.tableName("STAGE");
            TruncateStageTable.Connection = this.Conns["Commercial_STG"];

            EzDataFlow ExtractAllRecords = new StageDataFlow(this, this);
            ExtractAllRecords.Name = "Extract All Records";
            ExtractAllRecords.AttachTo(TruncateStageTable);

            this.SaveToFile(this.fileName());
            this.addConfigurations();
            this.addLogging(ExtractAllRecords.Name);
        }

    }

    class StageDataFlow : EzDataFlow
    {

        public PACKAGE_STAGE p;

        public EzOleDbSource Source;

        public EzLookup GetDatasetID;
        public EzLookup GetStageTableID;
        public EzChecksum GenerateChecksum;
        public EzLookup AssignMatchKey;

        public EzRowCount RowsMatched;
        public EzOleDbDestination MatchedDestination;

        public EzRowCount RowsInserted;
        public EzLookup RetrieveLastMatchKey;
        public EzRowNumber GenerateMatchKey;
        public EzDerivedColumn InsertNewMatchKey;

        public EzMultiCast MultiCast;

        public EzOleDbDestination InsertedDestination;
        public EzOleDbDestination MasterMatch;

        public string getNaturalKeyColumnName()
        {
            return "(DT_STR,150,1252)\"" + p.d.NATURALKEYCOLUMNS[0] + "\"";
        }

        public string getNaturalKey()
        {
            return "(DT_STR,150,1252)" + this.p.d.NATURALKEYCOLUMNS[0];
        }

        public StageDataFlow(EzContainer parent, PACKAGE_STAGE p)
            : base(parent)
        {
            this.p = p;

            Source = new EzOleDbSource(this);
            Source.Connection = p.Conns["Source"];
            //TODO allow custom query
            Source.SqlCommand = p.d.SOURCEQUERY;
            Source.Name = p.d.NAME;
            Console.WriteLine("Source Component created ...");

            EzDerivedColumn CreateNaturalKey = new EzDerivedColumn(this);
            CreateNaturalKey.AttachTo(Source);
            CreateNaturalKey.Name = "Create Natural Key";
            //TODO split NaturalKey from NaturalKeyColumnName, generate NaturalKey expression programmatically
            CreateNaturalKey.Expression["NaturalKey"] = this.getNaturalKey();
            CreateNaturalKey.Expression["NaturalKeyColumnName"] = this.getNaturalKeyColumnName();
            CreateNaturalKey.Expression["CreateDate"] = "GETDATE()";
            CreateNaturalKey.Expression["StageTableName"] = "(DT_STR,150,1252)\"" + p.tableName("STAGE") + "\"";
            CreateNaturalKey.Expression["DataSetName"] = "(DT_STR,150,1252)\"" + p.d.MATCHDATASET + "\"";
            Console.WriteLine("Derived Columns added ...");

            GetDatasetID = new EzLookup(this);
            GetDatasetID.Name = "Get Dataset ID";
            GetDatasetID.AttachTo(CreateNaturalKey);
            GetDatasetID.SetJoinCols("DataSetName,DataSetName");
            GetDatasetID.OleDbConnection = this.p.Conns["Commercial_MATCH"];
            GetDatasetID.SqlCommand = "select DataSetName, DataSetID from DataSets";
            GetDatasetID.SetCopyOverwriteCols("DataSetID,DataSetID");
            Console.WriteLine("DataSet ID Acquired ...");

            //TODO if DataSetid is 0, insert and return value

            GetStageTableID = new EzLookup(this);
            GetStageTableID.Name = "Get Stage Table ID";
            GetStageTableID.AttachTo(GetDatasetID);
            GetStageTableID.SetJoinCols("StageTableName,TableName");
            GetStageTableID.OleDbConnection = p.Conns["Commercial_META"];
            GetStageTableID.SqlCommand = "select TableName, TableID from meta_Table";
            GetStageTableID.SetCopyOverwriteCols("Stage_TableID,TableID");
            Console.WriteLine("Stage Table ID Acquired...");

            //TODO if StageTableID is 0, insert and return value

            GenerateChecksum = new EzChecksum(this);
            GenerateChecksum.Name = "Generate Checksum";
            GenerateChecksum.LinkAllInputsToOutputs();
            GenerateChecksum.ReinitializeMetaData();
            GenerateChecksum.AttachTo(GetStageTableID);

            AssignMatchKey = new EzLookup(this);
            AssignMatchKey.Name = "Assign Match Key";
            AssignMatchKey.AttachTo(GenerateChecksum);
            AssignMatchKey.SetJoinCols("NaturalKey,NaturalKey");
            AssignMatchKey.OleDbConnection = p.Conns["Commercial_MATCH"];
            AssignMatchKey.SqlCommand = "SELECT NaturalKey, MatchKey from " + this.p.tableName("MATCH");
            AssignMatchKey.SetCopyOverwriteCols("MatchKey,MatchKey");
            AssignMatchKey.NoMatchBehavor = NoMatchBehavior.SendToNoMatchOutput;
            Console.WriteLine("Matchkey checked ...");

            RowsMatched = new EzRowCount(this);
            RowsMatched.Name = "Rows Matched";
            RowsMatched.VariableName = "Audit::RowsMatched";
            RowsMatched.AttachTo(AssignMatchKey, 0, 0);

            MatchedDestination = new EzOleDbDestination(this);
            MatchedDestination.Name = this.p.tableName("STAGE");
            MatchedDestination.AttachTo(RowsMatched);
            MatchedDestination.Connection = this.p.Conns["Commercial_STG"];
            MatchedDestination.Table = this.p.tableName("STAGE");
            MatchedDestination.LinkAllInputsToOutputs();
            MatchedDestination.ReinitializeMetaData();
            Console.WriteLine("Matched Destination created ...");

            RowsInserted = new EzRowCount(this);
            RowsInserted.Name = "Rows Inserted";
            RowsInserted.VariableName = "Audit::RowsInserted";
            RowsInserted.AttachTo(AssignMatchKey, 1, 0);

            RetrieveLastMatchKey = new EzLookup(this);
            RetrieveLastMatchKey.Name = "Retrieve Last MatchKey";
            RetrieveLastMatchKey.AttachTo(RowsInserted);
            RetrieveLastMatchKey.SetJoinCols("DataSetID,DataSetID");
            RetrieveLastMatchKey.OleDbConnection = this.p.Conns["Commercial_MATCH"];
            RetrieveLastMatchKey.SqlCommand = String.Format("SELECT DataSetID, MAX(MatchKey) as MaxMatchKey FROM {0} GROUP BY DataSetID", this.p.tableName("MATCH"));
            RetrieveLastMatchKey.SetCopyOverwriteCols("MaxMatchKey,MaxMatchKey");

            GenerateMatchKey = new EzRowNumber(this);
            GenerateMatchKey.Name = "Generate MatchKey";
            GenerateMatchKey.AttachTo(RetrieveLastMatchKey);

            InsertNewMatchKey = new EzDerivedColumn(this);
            InsertNewMatchKey.Name = "Insert New MatchKey";
            InsertNewMatchKey.AttachTo(GenerateMatchKey);
            InsertNewMatchKey.Expression["MatchKey"] = "ISNULL(MaxMatchKey) ==  TRUE  ? RowNumber : MaxMatchKey + RowNumber";
            Console.WriteLine("New Matchkey component created ...");

            MultiCast = new EzMultiCast(this);
            MultiCast.Name = "MultiCast";
            MultiCast.AttachTo(InsertNewMatchKey);

            InsertedDestination = new EzOleDbDestination(this);
            InsertedDestination.Name =String.Format("{0} - New Records",  this.p.tableName("STAGE"));
            InsertedDestination.AttachTo(MultiCast, 0, 0);
            InsertedDestination.Connection = p.Conns["Commercial_STG"];
            InsertedDestination.Table = p.tableName("STAGE");
            InsertedDestination.LinkAllInputsToOutputs();
            InsertedDestination.ReinitializeMetaData();

            MasterMatch = new EzOleDbDestination(this);
            MasterMatch.Name = this.p.tableName("MATCH");
            MasterMatch.AttachTo(MultiCast, 1, 0);
            MasterMatch.Connection = p.Conns["Commercial_MATCH"];
            MasterMatch.ReinitializeMetaData();
            MasterMatch.Table = this.p.tableName("MATCH");
            MasterMatch.LinkAllInputsToOutputs();
            
            Console.WriteLine("Mastermatch Destination created ...");

        }

    }

   

}

   
   


