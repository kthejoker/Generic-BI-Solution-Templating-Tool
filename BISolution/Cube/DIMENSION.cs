using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Static;
using WpfApplication1.Mappings;
using DataConnection = WpfApplication1.DataConnection.DataConnection;



namespace WpfApplication1.Cube
{
	public class DIMENSION : DataConnection.DataObject2
    {
		
		private MAPPING _mappingField;
		
		[System.Xml.Serialization.XmlIgnore]
		public MAPPING MAPPING { get { 
				if (_mappingField == null) {
					_mappingField = this.cube.s.getMapping(this.NAME, "DIMENSION");
				}
				
				return _mappingField;} 
				set {
				
				_mappingField = value; 
				
				}
		}

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
        
        


        new public List<DataConnection.DataConnection.Column> getColumns() 
        {

            List<DataConnection.DataConnection.Column> columns = new List<DataConnection.DataConnection.Column>();
            // Add an identity column.
            columns.Add(new DataConnection.DataConnection.Column(this.NAME + "_ID", "int"));
            foreach (ATTRIBUTE a in this.ATTRIBUTES) {
            	columns.Add(a.c);
            }

			return columns;
              
        }



    
    }
}
