using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1.Mappings
{
    public class MAPPINGCOLUMN
    {

        [System.Xml.Serialization.XmlAttribute("DATACOLUMN")]
        public string DATACOLUMN { get; set; }

        [System.Xml.Serialization.XmlAttribute("ATTRIBUTE")]
        public string ATTRIBUTE { get; set; }

    }
}
