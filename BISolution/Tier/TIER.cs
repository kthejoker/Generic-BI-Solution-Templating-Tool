using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1.Tier
{
    public class TIER
    {

        [System.Xml.Serialization.XmlElement("NAME")]
        public string NAME { get; set; }

        [System.Xml.Serialization.XmlElement("SSMS", typeof(TIER_SSMS))]
        public TIER_SSMS SSMS { get; set; }

        [System.Xml.Serialization.XmlElement("SSIS", typeof(TIER_SSIS))]
        public TIER_SSIS SSIS { get; set; }

        public TIER_SSMS_DATABASE getDatabaseByLayer(string layerName)
        {
            return this.SSMS.DATABASEList.Find(delegate(TIER_SSMS_DATABASE tempDatabase)
            {
                return tempDatabase.LAYER == layerName;
            });

        }

        public TIER()
        {


        }
    }
}
