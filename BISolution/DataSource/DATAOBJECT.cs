using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Static;
using System.Data.SqlClient;

namespace WpfApplication1.DataSource
{
	public class DATAOBJECT : DataConnection.DataObject2
    {

 
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
        
        		
        new public List<WpfApplication1.DataConnection.DataConnection.Column> getColumns()
        {
            return this.ds.DataConnection.getColumns(this.SOURCEQUERY);
            
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
        
        public string tableName(string tableType)
        {
            string tableName = "";
         
            switch (tableType)
            {
                case "DIM":
                    tableName = String.Format("{0}_{1}", Prefixes.Prefix[tableType], this.NAME);
                    break;
                default:
                    tableName = String.Format("{0}_{1}_{2}", Prefixes.Prefix[tableType], this.ds.NAME, this.NAME);
                    break;
            }
            return tableName;
        }

    }
}
