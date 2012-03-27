using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Static;
using WpfApplication1.DataConnection;

namespace WpfApplication1.Cube
{
	public class DIMENSION : DataConnection.DataObject2
    {

        public DIMENSION() {
        	this.OBJECTTYPE = "DIMENSION";
        	this.DATABASELAYER = "DIM";
        }

        [System.Xml.Serialization.XmlElement("DATASET")]
        public string DATASET { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ATTRIBUTE", typeof(ATTRIBUTE), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ATTRIBUTE[] ATTRIBUTES
        {
            get;
            set;
        }


        new public List<WpfApplication1.DataConnection.DataConnection.Column> getColumns() 
        {

            List<WpfApplication1.DataConnection.DataConnection.Column> columns = new List<WpfApplication1.DataConnection.DataConnection.Column>();
            columns.Add(new DataConnection.DataConnection.Column(this.NAME + "_ID", "int"));
            foreach (ATTRIBUTE a in this.ATTRIBUTES) {
            	columns.Add(a.c);
            }

			return columns;
              
        }



    
    }
}
