using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1.Mappings
{
    public class SOURCEOBJECT
    {

        [System.Xml.Serialization.XmlAttribute("DATASOURCE")]
        public string NAME { get; set; }

        [System.Xml.Serialization.XmlAttribute("DATAOBJECT")]
        public string TYPE { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("MAPPINGCOLUMN", typeof(MAPPINGCOLUMN), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public MAPPINGCOLUMN[] MAPPINGCOLUMNS
        {
            get;
            set;
        }

    }
}
