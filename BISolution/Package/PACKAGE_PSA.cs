using System;
using Microsoft.SqlServer.SSIS.EzAPI;
using WpfApplication1.EzCustom;
using WpfApplication1.DataSource;

namespace WpfApplication1.Package
{
    class PACKAGE_PSA : PACKAGE
    {

 
        public PACKAGE_PSA(DATAOBJECT d) : base(d)
        {

            this.packageType = "PSA";
            this.Name = this.tableName();

            this.addConnection("Commercial_STG", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("STAGE").NAME);
            this.addConnection("Commercial_PSA", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("PSA").NAME);
            this.addConnection("Commercial_RUN", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("RUN").NAME);
            this.addConnection("Commercial_MATCH", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("MATCH").NAME);
            this.addConnection("Commercial_META", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("META").NAME);

            this.addVariable("Audit::RowsUpdated");
            this.addVariable("Audit::RowsInserted");

            EzDataFlow ExtractAllRecords = new PSADataFlow(this, this);
            ExtractAllRecords.Name = "Import Staging Data To PSA";

            this.SaveToFile(this.fileName());
            this.addConfigurations();
            this.addLogging(ExtractAllRecords.Name);

            //TODO Update Inactive Records
            //TOO Delete Processed Inactive Rows
        }

    }

    class PSADataFlow : EzDataFlow
    {

        public PACKAGE_PSA p;
        public EzOleDbSource Source;

        public EzOleDbDestination InsertedDestination;
        public EzOleDbDestination UpdatedDestination;
        public EzOleDbDestination MarkInactive;

        public EzDerivedColumn AddAuditColumns(EzComponent attacher)
        {

            EzDerivedColumn component = new EzDerivedColumn(this);
            component.AttachTo(attacher);
            component.Name = "Add Audit Columns";
            component.Expression["CreatedDate"] = "GETDATE()";
            component.Expression["ActiveFlag"] = "(DT_STR,1,1252)\"Y\"";
            return component;


        }

        public EzLookup ExistingChecksum(EzComponent attacher)
        {
            EzLookup component = new EzLookup(this);
            component.AttachTo(attacher);
            component.Name = "Lookup - Existing Checksum";
            component.OleDbConnection = this.p.Conns["Commercial_PSA"];
            component.SqlCommand = "select MatchKey, Checksum from dbo.[" + this.p.tableName() + "] where ActiveFlag ='Y'";
            component.SetJoinCols("MatchKey,MatchKey");
            component.SetPureCopyCols("Checksum");
            return component;

        }

        public EzConditionalSplit CompareChecksums()
        {
            EzConditionalSplit component = new EzConditionalSplit(this);
            component.Name = "Compare Checksums";
            //TODO actually compare checksums
            return component;
        }

        public EzMultiCast Multicast()
        {
            EzMultiCast component = new EzMultiCast(this);
            component.Name = "Multicast";
            return component;
        }

        public EzRowCount RowsUpdated()
        {
            EzRowCount component = new EzRowCount(this);
            component.Name = "Rows Updated";
            component.VariableName = "Audit::RowsUpdated";
            return component;
        }

        public EzDerivedColumn AddUpdateActionCode()
        {
            EzDerivedColumn component = new EzDerivedColumn(this);
            component.Name = "Add UPDATE Action Code";
            component.Expression["ActionCode"] = "(DT_STR,1,1252)\"UPDATE\"";
            return component;
        }

        public EzRowCount RowsInserted()
        {
            EzRowCount component = new EzRowCount(this);
            component.Name = "Rows Inserted";
            component.VariableName = "Audit::RowsInserted";
            return component;
        }

        public EzDerivedColumn AddInsertActionCode()
        {
            EzDerivedColumn component = new EzDerivedColumn(this);
            component.Name = "Add INSERT Action Code";
            component.Expression["ActionCode"] = "(DT_STR,1,1252)\"INSERT\"";
            return component;
        }

        public string getNaturalKeyColumnName()
        {
            return "";
        }

        public PSADataFlow(EzContainer parent, PACKAGE_PSA p)
            : base(parent)
        {

            this.p = p;

            Source = new EzOleDbSource(this);
            Source.Connection = this.p.Conns["Commercial_STG"];
            Source.SqlCommand = String.Format("select * from {0}", this.p.tableName("STAGE") );
            Source.Name = this.p.tableName("STAGE");
            Console.WriteLine("Source Component created ...");

            EzDerivedColumn AddAuditColumns = this.AddAuditColumns(Source);
            Console.WriteLine("Derived Columns added ...");

            EzLookup ExistingChecksum = this.ExistingChecksum(AddAuditColumns);
            ExistingChecksum.NoMatchBehavor = NoMatchBehavior.SendToNoMatchOutput;

            EzConditionalSplit CompareChecksums = this.CompareChecksums();
            CompareChecksums.AttachTo(ExistingChecksum, 0, 0);

            EzMultiCast Multicast = this.Multicast();
            Multicast.AttachTo(CompareChecksums, 0, 0);

            EzRowCount RowsUpdated = this.RowsUpdated();
            RowsUpdated.AttachTo(Multicast, 0, 0);

            EzDerivedColumn AddUpdateActionCode = this.AddUpdateActionCode();
            AddUpdateActionCode.AttachTo(RowsUpdated);

            EzRowCount RowsInserted = this.RowsInserted();
            RowsInserted.AttachTo(ExistingChecksum, 1, 0);

            EzDerivedColumn AddInsertActionCode = this.AddInsertActionCode();
            AddInsertActionCode.AttachTo(RowsInserted);

            MarkInactive = new EzOleDbDestination(this);
            MarkInactive.Name = "Mark Inactive Staging Table";
            MarkInactive.AttachTo(Multicast, 1, 0);
            MarkInactive.Connection = this.p.Conns["Commercial_PSA"];
            MarkInactive.Table = "psa_MarkInactive";
            MarkInactive.LinkAllInputsToOutputs();
            MarkInactive.ReinitializeMetaData();

            UpdatedDestination = new EzOleDbDestination(this);
            UpdatedDestination.Name = "PSA Data - Updates";
            UpdatedDestination.AttachTo(AddUpdateActionCode);
            UpdatedDestination.Connection = this.p.Conns["Commercial_PSA"];
            UpdatedDestination.Table = this.p.tableName();
            UpdatedDestination.LinkAllInputsToOutputs();
            UpdatedDestination.ReinitializeMetaData();

            InsertedDestination = new EzOleDbDestination(this);
            InsertedDestination.Name = "PSA Data - Inserts";
            InsertedDestination.AttachTo(AddInsertActionCode);
            InsertedDestination.Connection = this.p.Conns["Commercial_PSA"];
            InsertedDestination.Table = this.p.tableName();
            InsertedDestination.LinkAllInputsToOutputs();
            InsertedDestination.ReinitializeMetaData();

        }

    }
}
