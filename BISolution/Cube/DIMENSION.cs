using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Static;
using WpfApplication1.DataConnection;

namespace WpfApplication1.Cube
{
    public class DIMENSION
    {

        [System.Xml.Serialization.XmlIgnore]
        public CUBE cube;

        [System.Xml.Serialization.XmlIgnore]
        private WpfApplication1.DataConnection.DataConnection DataConnection_field;

        [System.Xml.Serialization.XmlIgnore]
        public WpfApplication1.DataConnection.DataConnection dc
        {
            get
            {
                if (this.DataConnection_field == null)
                {
                    this.DataConnection_field = this.cube.s.getCurrentTier().getDatabaseByLayer("DIM").DataConnection;
                }
                return this.DataConnection_field;
            }
            set { this.DataConnection_field = value; }
        }

        [System.Xml.Serialization.XmlAttribute("NAME")]
        public string NAME { get; set; }

        [System.Xml.Serialization.XmlElement("DATASET")]
        public string DATASET { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ATTRIBUTE", typeof(ATTRIBUTE), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ATTRIBUTE[] ATTRIBUTES
        {
            get;
            set;
        }


        public List<WpfApplication1.DataConnection.DataConnection.Column> getColumns() 
        {

              List<WpfApplication1.DataConnection.DataConnection.Column> columns = new List<WpfApplication1.DataConnection.DataConnection.Column>();



              columns.Add(new DataConnection.DataConnection.Column(this.NAME + "_ID", "int"));

                foreach (ATTRIBUTE a in this.ATTRIBUTES)
                {
                    columns.Add(a.c);

                }

                return columns;
              
               
        }

        public void createDimTable(bool dropExisting = false)
        {

            string databaseLayer = "DIM";
           ;

            string tableName = Prefixes.Prefix[databaseLayer] + "_" + this.NAME;
            string fixedColumns = Columns.Column[databaseLayer];
            string tableColumns = "";

            foreach (WpfApplication1.DataConnection.DataConnection.Column c in this.getColumns())
            {
                tableColumns = tableColumns + c.convertToSQL() + ", ";
            }
            string columnInfo = tableColumns + fixedColumns;

            if (dropExisting)
            {
                this.dc.dropTable(tableName);
            }
            this.dc.createTable(tableName, columnInfo);

        }

    
    }
}
