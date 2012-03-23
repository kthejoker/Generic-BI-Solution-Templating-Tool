using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.DataConnection;

namespace WpfApplication1.Tier
{


    public class TIER_SSMS_DATABASE
    {

        [System.Xml.Serialization.XmlIgnore]
        private WpfApplication1.DataConnection.DataConnection DataConnection_field;

        public string DATABASENAME { get { return this.NAME; } set { this.NAME = value; } }

        public TIER_SSMS Parent;

        [System.Xml.Serialization.XmlElementAttribute("NAME", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NAME
        {
            get;
            set;
        }

        public TIER_SSMS_DATABASE()
        {
           

        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CONNECTIONSTRING", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CONNECTIONSTRING
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlIgnore]
        public WpfApplication1.DataConnection.DataConnection DataConnection
        {
            get {
                if (this.DataConnection_field == null)
                {
                    this.DataConnection_field = new WpfApplication1.DataConnection.DataConnection(this.Parent.SERVERNAME, this.DATABASENAME);
                }
                return this.DataConnection_field; }
            set { this.DataConnection_field = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("LAYER")]
        public string LAYER
        {
            get;
            set;
        }

    }
}
