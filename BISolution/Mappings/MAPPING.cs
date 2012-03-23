using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1.Mappings
{
    public class MAPPING
    {

        [System.Xml.Serialization.XmlAttribute("NAME")]
        public string NAME { get; set; }

        [System.Xml.Serialization.XmlAttribute("TYPE")]
        public string TYPE { get; set; }

         [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SOURCEOBJECT", typeof(SOURCEOBJECT), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public SOURCEOBJECT[] SOURCEOBJECTS
        {
            get;
            set;
        }
    }
}
