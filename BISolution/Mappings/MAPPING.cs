using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1.Mappings
{
    public class MAPPING
    {
    	
    	private SOURCEOBJECT[] _sourceObjectField;
    	private LOOKUP[] _lookupField;
    	
    	public SOLUTION s;

        [System.Xml.Serialization.XmlAttribute("NAME")]
        public string NAME { get; set; }

        [System.Xml.Serialization.XmlAttribute("TYPE")]
        public string TYPE { get; set; }

         [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SOURCEOBJECT", typeof(SOURCEOBJECT), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public SOURCEOBJECT[] SOURCEOBJECTS
        {
             get { return _sourceObjectField; }
            set
            {
                foreach (SOURCEOBJECT so in value)
                {
                    so.mapping = this;
                }
                _sourceObjectField = value;
            }
        }
        
                 [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
                 [System.Xml.Serialization.XmlArrayItemAttribute("LOOKUP", typeof(LOOKUP), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public LOOKUP[] LOOKUPS
        {
             get { return _lookupField; }
            set
            {
                foreach (LOOKUP lu in value)
                {
                    lu.mapping = this;
                }
                _lookupField = value;
            }
        }
        
        
        
    }
}
