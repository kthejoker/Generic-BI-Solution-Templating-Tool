using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.SSIS.EzAPI;
using Microsoft.SqlServer.Dts;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask;
using WpfApplication1.Tier;
using WpfApplication1.DataSource;
using WpfApplication1.DataConnection;
using WpfApplication1.Static;
using WpfApplication1.Cube;

namespace WpfApplication1.Package
{
    class PACKAGE : EzPackage
    {

        public Dictionary<string, EzSqlOleDbCM> Conns = new Dictionary<string, EzSqlOleDbCM>();
        public SOLUTION s;
        public TIER t;
        public DATAOBJECT d;
        public DIMENSION dim;
        public string packageType;

        public void addVariable(string VariableName, int DefaultValue = 0, bool readOnly = false)
        {
            string[] nameSpace = VariableName.Split(new string[] { "::" }, StringSplitOptions.None);
            this.Variables.Add(nameSpace[1], readOnly, nameSpace[0], DefaultValue);
            //TODO Scope in Variables window shows Class ID instead of namespace's name
        }

        public void addConfigurations()
        {

            Application app = new Application();
            Microsoft.SqlServer.Dts.Runtime.Package p = app.LoadPackage(this.fileName(), null);

            p.EnableConfigurations = true;

            foreach (var conn in this.Conns)
            {
                Microsoft.SqlServer.Dts.Runtime.Configuration config = p.Configurations.Add();
                config.Name = this.t.SSIS.CONNECTIONPREFIX + conn.Value.Name;
                config.ConfigurationType = DTSConfigurationType.ConfigFile;
                config.ConfigurationString = String.Format(@"{0}\{1}.dtsConfig", this.t.SSIS.PACKAGECONFIGURATIONFOLDER, config.Name);
            }


            //Configuration connect_Commercial_STG = p.Configurations.Add();
            //connect_Commercial_STG.Name = "connect_Commercial_STG";
            //connect_Commercial_STG.ConfigurationType = DTSConfigurationType.ConfigFile;
            //connect_Commercial_STG.ConfigurationString = @"C:\CommercialBI\SSIS\Config\connect_Commercial_STG.dtsConfig";

            //Configuration connect_Commercial_PSA = p.Configurations.Add();
            //connect_Commercial_PSA.Name = "connect_Commercial_PSA";
            //connect_Commercial_PSA.ConfigurationType = DTSConfigurationType.ConfigFile;
            //connect_Commercial_PSA.ConfigurationString = @"C:\CommercialBI\SSIS\Config\connect_Commercial_PSA.dtsConfig";

            //Configuration connect_Commercial_RUN = p.Configurations.Add();
            //connect_Commercial_RUN.Name = "connect_Commercial_RUN";
            //connect_Commercial_RUN.ConfigurationType = DTSConfigurationType.ConfigFile;
            //connect_Commercial_RUN.ConfigurationString = @"C:\CommercialBI\SSIS\Config\connect_Commercial_RUN.dtsConfig";

            //Configuration connect_Commercial_USER = p.Configurations.Add();
            //connect_Commercial_USER.Name = "connect_Commercial_USER";
            //connect_Commercial_USER.ConfigurationType = DTSConfigurationType.ConfigFile;
            //connect_Commercial_USER.ConfigurationString = @"C:\CommercialBI\SSIS\Config\connect_Commercial_USER.dtsConfig";

            //Configuration connect_Commercial_DIM = p.Configurations.Add();
            //connect_Commercial_DIM.Name = "connect_Commercial_DIM";
            //connect_Commercial_DIM.ConfigurationType = DTSConfigurationType.ConfigFile;
            //connect_Commercial_DIM.ConfigurationString = @"C:\CommercialBI\SSIS\Config\connect_Commercial_DIM.dtsConfig";

            //Configuration connect_Commercial_FACT = p.Configurations.Add();
            //connect_Commercial_FACT.Name = "connect_Commercial_FACT";
            //connect_Commercial_FACT.ConfigurationType = DTSConfigurationType.ConfigFile;
            //connect_Commercial_FACT.ConfigurationString = @"C:\CommercialBI\SSIS\Config\connect_Commercial_FACT.dtsConfig";

            app.SaveToXml(this.fileName(), p, null);

        }

        public void addConnection(string connectionName, string connectionString)
        {
            this.Conns.Add(connectionName, new EzSqlOleDbCM(this, connectionName));
            this.Conns[connectionName].ConnectionString = connectionString;

        }

        public void addConnection(string connectionName, string servername, string db)
        {
            this.Conns.Add(connectionName, new EzSqlOleDbCM(this, connectionName));
            this.Conns[connectionName].SetConnectionString(servername, db);

        }

        public string tableName(string packageType = null)
        {
            string tableName = "";
            if (packageType == null) { packageType = this.packageType; }
            switch (this.packageType)
            {
                case "DIM":
                    tableName = String.Format("{0}_{1}", Prefixes.Prefix[packageType], this.d.NAME);
                    break;
                default:
                    tableName = String.Format("{0}_{1}_{2}", Prefixes.Prefix[packageType], this.d.ds.NAME, this.d.NAME);
                    break;
            }
            return tableName;
        }

        public string fileName()
        {
            return String.Format(@"C:\sandbox\{0}.dtsx",this.tableName());
        }

        public void addLogging(string Executable)
        {



            Application app = new Application();
            Microsoft.SqlServer.Dts.Runtime.Package p = app.LoadPackage(this.fileName(), null);

            TaskHost th = (p.Executables[Executable] as TaskHost);
            DtsEventHandler dh = (DtsEventHandler)th.EventHandlers.Add("OnPostExecute");

            Executable Logger = dh.Executables.Add("STOCK:SQLTask");
            TaskHost LoggerTH = Logger as TaskHost;
            LoggerTH.Name = "Log event";
            LoggerTH.Properties["SqlStatementSource"].SetExpression(LoggerTH, @"""INSERT INTO SSISLog (RunDate, EventType, PackageName, TaskName, EventCode, EventDescription, PackageDuration, ContainerDuration, MatchedInsertCount, UnMatchedInsertCount, UpdateCount, DeleteCount, Host)
VALUES ('""+ (DT_STR, 25, 1252) @[System::ContainerStartTime] + ""',	'OnPostExecute', '""+ @[System::PackageName]  + ""', '"" + @[System::SourceName]  + ""', 0, '',
	"" + (DT_STR, 6, 1252) DATEDIFF( ""ss"", @[System::StartTime] , GETDATE() )  + "",
	"" + (DT_STR, 6, 1252) DATEDIFF( ""ss"", @[System::ContainerStartTime] , GETDATE() ) + "",
	"" + (DT_STR, 24, 1252) @[Audit::RowsMatched]  + "",
	"" + (DT_STR, 24, 1252) @[Audit::RowsInserted]  + "",
	"" + (DT_STR, 24, 1252) @[Audit::RowsUpdated] + "", 
	"" + (DT_STR, 24, 1252) @[Audit::RowsDeleted] + "", 
	'"" + @[System::MachineName]  + ""')""");
            (LoggerTH.InnerObject as ExecuteSQLTask).Connection = "Commercial_RUN";
            //  LoggerTH.InnerObject
            (LoggerTH.InnerObject as ExecuteSQLTask).SqlStatementSource = @"Select 1";

            app.SaveToXml(this.fileName(), p, null);
            return;

        }

        public PACKAGE(DATAOBJECT d)
        {

            this.d = d;
            this.s = d.ds.s;
            this.t = s.getCurrentTier();
        }
        
        public PACKAGE(DIMENSION dim) {
          	this.dim = dim;
            this.s = dim.ds.s;
            this.t = s.getCurrentTier();
        }

    }
}
