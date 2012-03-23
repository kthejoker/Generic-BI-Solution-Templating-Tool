using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1.Cube
{
    public class CUBE
    {   

        public SOLUTION s;
        private DIMENSION[] DIMENSIONS_field;

        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DIMENSION", typeof(DIMENSION), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public DIMENSION[] DIMENSIONS
        {
            get { return this.DIMENSIONS_field; }
            set {

              

                foreach (DIMENSION dim in value)
                {
                    dim.cube = this;
                  
                }
                this.DIMENSIONS_field = value;
               
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("MEASURE", typeof(MEASURE), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public MEASURE[] MEASURES
        {
            get;
            set;
        }

    }
}
