using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data.SqlClient;

namespace WpfApplication1.DataSource
{
    public class DATASOURCE
    {

        public DATASOURCE()
        {
           
        }

        private DATAOBJECT[] DataObjectsField;

        public SOLUTION s;

        [System.Xml.Serialization.XmlIgnore]
        private WpfApplication1.DataConnection.DataConnection DataConnection_field;
        

        [System.Xml.Serialization.XmlAttribute("NAME")]
        public string NAME { get; set; }

        [System.Xml.Serialization.XmlAttribute("TYPE")]
        public string TYPE { get; set; }

        [System.Xml.Serialization.XmlElement("SERVERNAME")]
        public string SERVERNAME { get; set; }

        [System.Xml.Serialization.XmlElement("DATABASENAME")]
        public string DATABASENAME { get; set; }

        
        [System.Xml.Serialization.XmlElement("CONNECTIONSTRING")]
        public string CONNECTIONSTRING { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DATAOBJECT", typeof(DATAOBJECT), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public DATAOBJECT[] DATAOBJECTS
        {
            get { return this.DataObjectsField; }
            set
            {
                foreach (DATAOBJECT d in value)
                {
                    d.ds = this;
                }
                 this.DataObjectsField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public WpfApplication1.DataConnection.DataConnection DataConnection
        {
            get
            {
                if (this.DataConnection_field == null)
                {
                    this.DataConnection_field = new WpfApplication1.DataConnection.DataConnection(this.SERVERNAME, this.DATABASENAME);
                }
                return this.DataConnection_field;
            }
            set { this.DataConnection_field = value; }
        }


      
    }
}
