using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.DataSource;

namespace WpfApplication1.Package
{
    class PACKAGE_DIM : PACKAGE
    {

          public PACKAGE_DIM(DATAOBJECT d) : base(d)
        {
            this.packageType = "DIM";

            //this.Name = this.tableName("STAGE");

            //this.addConnection("Commercial_STG", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("STAGE").NAME);
            //this.addConnection("Commercial_PSA", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("PSA").NAME);
            //this.addConnection("Commercial_RUN", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("RUN").NAME);
            //this.addConnection("Commercial_MATCH", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("MATCH").NAME);
            //this.addConnection("Commercial_META", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("META").NAME);
            //this.addConnection("Source", this.d.ds.SERVERNAME, this.d.ds.DATABASENAME);

            //this.addVariable("Audit::RowsMatched");
            //this.addVariable("Audit::RowsInserted");
            //this.addVariable("Audit::RowsUpdated");
            //this.addVariable("Audit::RowsDeleted");
            ////System.Windows.MessageBox.Show(this.Conns["Commercial_STG"].ServerName);

            ////Clear stage table for clean full load of source data
            //EzExecuteSQLTask TruncateStageTable = new EzExecuteSQLTask(this);
            //TruncateStageTable.Name = "Truncate Stage Table";
            //TruncateStageTable.SqlStatementSource = "truncate table " + this.tableName("STAGE");
            //TruncateStageTable.Connection = this.Conns["Commercial_STG"];

            //EzDataFlow ExtractAllRecords = new StageDataFlow(this, this);
            //ExtractAllRecords.Name = "Extract All Records";
            //ExtractAllRecords.AttachTo(TruncateStageTable);

            //this.SaveToFile(this.fileName());
            //this.addConfigurations();
            //this.addLogging(ExtractAllRecords.Name);
        }

    }
}
