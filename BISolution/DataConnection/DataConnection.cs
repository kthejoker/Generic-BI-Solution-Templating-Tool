using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace WpfApplication1.DataConnection
{
    public class DataConnection
    {

        public string SERVERNAME;
        public string DATABASENAME;
        public string CONNECTIONSTRING;

        public SqlConnection connect()
        {
            SqlConnection myConnection = new SqlConnection(String.Format(@"server={0};database={1};integrated security=SSPI;", this.SERVERNAME, this.DATABASENAME));
            return myConnection;
        }

        public DataConnection(string SERVERNAME, string DATABASENAME)
        {
            this.SERVERNAME = SERVERNAME;
            this.DATABASENAME = DATABASENAME;
        }

        public SqlDataReader reader;

        public class Column
        {

            public string ColumnName;
            public string ColumnType;
            public string ColumnLength;

            public string convertToSQL(bool isNull = true)
            {
                return this.ColumnName + " " + this.ColumnType + (this.ColumnLength.Length > 0 && !this.ColumnType.Equals("int") ? "(" + this.ColumnLength + ")" : "" ) + (isNull ? " NULL" : " NOT NULL");

            }

            public Column(string ColumnName, string ColumnType, string ColumnLength = "")
            {
                this.ColumnLength = ColumnLength;
                this.ColumnName = ColumnName;
                this.ColumnType = ColumnType;
                
            }
        }

        public List<Column> getColumns(string queryToRun)
        {

            using (SqlConnection connection = this.connect())
            {
                SqlCommand command = new SqlCommand(queryToRun, connection);
                connection.Open();

                List<Column> columns = new List<Column>();
                this.reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
                DataTable dt = this.reader.GetSchemaTable();
              
                foreach (DataRow r in dt.Rows)
                {
                    
                   string ColumnName = r[dt.Columns[dt.Columns.IndexOf("ColumnName")]].ToString();
                        string ColumnType = r[dt.Columns[dt.Columns.IndexOf("DataTypeName")]].ToString();
                        string MaxLength = r[dt.Columns[dt.Columns.IndexOf("ColumnSize")]].ToString();

                       columns.Add(new Column(ColumnName, ColumnType, MaxLength));
                  
  
                }
                this.reader.Close();
                
                return columns;
            }

        }

        public void dropTable(string tableToDrop)
        {

            string dropTableScript = String.Format("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U')) DROP TABLE [{0}]", tableToDrop);
            this.runQuery(dropTableScript);
            this.reader.Close();
        }

        public void createTable(string tableToCreate, string columnInfo)
        {

            string createTableScript = String.Format("BEGIN TRANSACTION GO IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U')) CREATE TABLE dbo.{0} ({1} ) COMMIT", tableToCreate, columnInfo);
            this.runQuery(createTableScript);
            this.reader.Close();

        }

        public SqlDataReader runQuery(string queryToRun)
        {


            using (SqlConnection connection =
                      this.connect())
            {
                SqlCommand command =
                    new SqlCommand(queryToRun, connection);
                connection.Open();

                this.reader = command.ExecuteReader();

                // Call Close when done reading.
                return this.reader;
            }
        }

    }
}
