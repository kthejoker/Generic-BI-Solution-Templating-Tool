using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WpfApplication1.Tier
{
    public class TIER_SSMS
    {

        private TIER_SSMS_DATABASE[] DATABASES_field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("SERVERNAME", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SERVERNAME
        {
            get;
            set;
        }

        /// <remarks/>
        /// 
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DATABASE", typeof(TIER_SSMS_DATABASE), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public TIER_SSMS_DATABASE[] DATABASES
        {
            get
            {
                return this.DATABASES_field;
            }
            set
            {
                foreach (TIER_SSMS_DATABASE db in value)
                {
                    db.Parent = this;
                }

                this.DATABASES_field = value;
            }
        }

        [XmlIgnore]
        public List<TIER_SSMS_DATABASE> DATABASEList { get { return new List<TIER_SSMS_DATABASE>(DATABASES); } }

        public TIER_SSMS()
        {
        }
    }
}
