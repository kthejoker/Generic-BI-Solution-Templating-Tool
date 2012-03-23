using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Static;
using System.Data.SqlClient;

namespace WpfApplication1.DataSource
{
    public class DATAOBJECT
    {
        public DATASOURCE ds;

          public DATAOBJECT()
        {
        }

        public DATAOBJECT(DATASOURCE ds)

        {
            this.ds = ds;
            
        }

        public List<WpfApplication1.DataConnection.DataConnection.Column> getColumns()
        {
            return this.ds.DataConnection.getColumns(this.SOURCEQUERY);
            
        }

        public void createTable(string databaseLayer, bool dropExisting = false)
        {
            DataConnection.DataConnection dc = this.ds.s.getCurrentTier().getDatabaseByLayer(databaseLayer).DataConnection;

            string tableName = Prefixes.Prefix[databaseLayer] + this.ds.NAME + "_" + this.NAME;
            string fixedColumns = Columns.Column[databaseLayer];
            string tableColumns = "";
            if (!databaseLayer.Equals("MATCH"))
            {
                foreach (WpfApplication1.DataConnection.DataConnection.Column c in this.getColumns())
                {
                    tableColumns = tableColumns + c.convertToSQL() + ", ";
                }
            }
            string columnInfo = tableColumns + fixedColumns;

            if (dropExisting)
            {
                dc.dropTable(tableName);
            }
            dc.createTable(tableName, columnInfo);

        }

        public void createStageTable(bool dropExisting = false)
        {
            createTable("STAGE", dropExisting);
    
        }

        public void createPSATable(bool dropExisting = false)
        {
            createTable("PSA", dropExisting);

        }

        public void createMatchTable(bool dropExisting = false)
        {
            createTable("MATCH", dropExisting);

        }

       

        [System.Xml.Serialization.XmlAttribute("NAME")]
        public string NAME { get; set; }

        [System.Xml.Serialization.XmlAttribute("TYPE")]
        public string TYPE { get; set; }


        [System.Xml.Serialization.XmlElement("SOURCEQUERY")]
        public string SOURCEQUERY { get; set; }

        [System.Xml.Serialization.XmlElement("MATCHDATASET")]
        public string MATCHDATASET { get; set; }


        [System.Xml.Serialization.XmlElement("LOADTYPE")]
        public string LOADTYPE { get; set; }

        [System.Xml.Serialization.XmlElement("TIMESTAMPCOLUMN")]
        public string TIMESTAMPCOLUMN { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute("NATURALKEY", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("NATURALKEYCOLUMN", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] NATURALKEYCOLUMNS
        {
            get;
            set;
        }



    }
}
