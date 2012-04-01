using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataConnection = WpfApplication1.DataConnection.DataConnection;
using WpfApplication1.Static;

namespace WpfApplication1.Cube
{
    public class ATTRIBUTE
    {
        [System.Xml.Serialization.XmlIgnore]
        private DataConnection.DataConnection.Column c_FIELD;

        [System.Xml.Serialization.XmlIgnore]
        public DataConnection.DataConnection.Column c {
                get { 
                        if (this.c_FIELD == null) {
                            this.DATATYPE = DimensionAttributeTypes.DimensionAttributeType[this.TYPE].Find(delegate(KeyValuePair<string, string> kv) { return kv.Key == "DataType"; }).Value;
                            this.LENGTH = DimensionAttributeTypes.DimensionAttributeType[this.TYPE].Find(delegate(KeyValuePair<string, string> kv) { return kv.Key == "Length"; }).Value;
                            this.c_FIELD = new DataConnection.DataConnection.Column(this.NAME, this.DATATYPE, this.LENGTH);
                        }
                    return this.c_FIELD; 
                }
            set { this.c_FIELD = value; } 
        }

        [System.Xml.Serialization.XmlIgnore]
        public string DATATYPE;

        [System.Xml.Serialization.XmlIgnore]
        public string LENGTH;

        [System.Xml.Serialization.XmlAttribute("TYPE")]
        public string TYPE { get; set; }

        [System.Xml.Serialization.XmlAttribute("NAME")]
        public string NAME { get; set; }

    }
}
