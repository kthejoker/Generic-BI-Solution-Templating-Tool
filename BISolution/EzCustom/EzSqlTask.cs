using System;
using Microsoft.SqlServer.SSIS.EzAPI;
using Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask;
using Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask.Connections;
using Microsoft.SqlServer.Dts.Runtime;

namespace WpfApplication1.EzCustom
{
    [ExecID("STOCK:SQLTask")]
    public class EzExecuteSQLTask : EzTask
    {

        public EzExecuteSQLTask(EzContainer parent) : base(parent) { }

        public EzExecuteSQLTask(EzContainer parent, TaskHost task) : base(parent, task) { }

        protected EzConnectionManager m_connection;

        public string SqlStatementSource { get; set; }

        public EzConnectionManager Connection
        {

            get { return m_connection; }
            set
            {
                (host.InnerObject as ExecuteSQLTask).Connection = value.Name;
                m_connection = value;
            }

        }



    }
}
