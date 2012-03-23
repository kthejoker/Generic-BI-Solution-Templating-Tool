using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1.Tier
{
    public class TIER_SSIS
    {
         [System.Xml.Serialization.XmlElement("PACKAGECONFIGURATIONFOLDER", typeof(string))]
        public string PACKAGECONFIGURATIONFOLDER {get; set;}

         [System.Xml.Serialization.XmlElement("CONNECTIONPREFIX", typeof(string))]
        public string CONNECTIONPREFIX { get; set; }

    }
}
