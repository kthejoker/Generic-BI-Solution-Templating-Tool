using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.DataSource;

namespace WpfApplication1.Mappings
{
    public class SOURCEOBJECT
    {
    	
    	public MAPPING mapping;
    	private DATAOBJECT _dataObjectField;
    	
    	public DATAOBJECT DATAOBJECT {
    		get {
    			if (_dataObjectField == null) {
    				_dataObjectField = this.mapping.s.getDataObject(this.DATASOURCENAME, this.DATAOBJECTNAME) ;
    			}
    			return _dataObjectField;
    		}
    		set {
    			_dataObjectField = value;
    		}
    	}

        [System.Xml.Serialization.XmlAttribute("DATASOURCENAME")]
        public string DATASOURCENAME { get; set; }

        [System.Xml.Serialization.XmlAttribute("DATAOBJECTNAME")]
        public string DATAOBJECTNAME { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("MAPPINGCOLUMN", typeof(MAPPINGCOLUMN), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public MAPPINGCOLUMN[] MAPPINGCOLUMNS
        {
            get;
            set;
        }

    }
}
