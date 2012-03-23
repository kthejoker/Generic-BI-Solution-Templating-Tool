using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Tier;
using System.Xml.Serialization;

namespace WpfApplication1.Configuration
{

   public class CONFIGURATION
    {
   	//New Comment

        private TIER[] tIERSField;

       [System.Xml.Serialization.XmlArrayAttribute("TIERS",Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("TIER", typeof(TIER), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public TIER[] TIERS { get { return tIERSField; } set { tIERSField = value; } }

        [XmlIgnore]
        public List<TIER> TIERList { get { return new List<TIER>(TIERS); } }

    }

}
