using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.DataSource;
using WpfApplication1.Configuration;
using WpfApplication1.Tier;
using WpfApplication1.Cube;
using WpfApplication1.Mappings;

namespace WpfApplication1
{
    public class SOLUTION
    {

        private DATASOURCE[] dataSourceField;
        private TIER currentTier;
        private CUBE CUBE_field;

        [System.Xml.Serialization.XmlElement("NAME")]
        public string NAME { get; set; }

        [System.Xml.Serialization.XmlElement("CONFIGURATION", typeof(CONFIGURATION))]
        public CONFIGURATION CONFIGURATION { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DATASOURCE", typeof(DATASOURCE), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public DATASOURCE[] DATASOURCES
        {
            get { return dataSourceField; }
            set
            {
                foreach (DATASOURCE ds in value)
                {
                    ds.s = this;
                }
                dataSourceField = value;
            }
        }

        [System.Xml.Serialization.XmlElement("CUBE", typeof(CUBE))]
        public CUBE CUBE { get { return this.CUBE_field; } set { this.CUBE_field = value; this.CUBE_field.s = this; } }

        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("MAPPING", typeof(MAPPING), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public MAPPING[] MAPPINGS
        {
            get;
            set;
        }

        public TIER getTier(string tierName)
        {

            this.currentTier = this.CONFIGURATION.TIERList.Find(delegate(TIER tempTier)
            {
                return tempTier.NAME == tierName;
            });
            return this.currentTier;
        }

        public TIER getCurrentTier()
        {
            return this.currentTier;
        }

        [System.Xml.Serialization.XmlIgnore]
        public List<DATASOURCE> DATASOURCEList { get { return new List<DATASOURCE>(DATASOURCES); } }

        public SOLUTION()
        {
        }
    }
}
