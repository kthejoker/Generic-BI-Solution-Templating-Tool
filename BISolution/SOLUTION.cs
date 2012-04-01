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

        private DATASOURCE[] _dataSourceField;
        private MAPPING[] _mappingField;
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
            get { return _dataSourceField; }
            set
            {
                foreach (DATASOURCE ds in value)
                {
                    ds.s = this;
                }
                _dataSourceField = value;
            }
        }

        [System.Xml.Serialization.XmlElement("CUBE", typeof(CUBE))]
        public CUBE CUBE { get { return this.CUBE_field; } set { this.CUBE_field = value; this.CUBE_field.s = this; } }

        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("MAPPING", typeof(MAPPING), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public MAPPING[] MAPPINGS
        {
             get { return _mappingField; }
            set
            {
                foreach (MAPPING m in value)
                {
                    m.s = this;
                }
                _mappingField = value;
            }
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
        
        public MAPPING getMapping(string mappingName, string mappingType) {
        	MAPPING M = Array.Find(this.MAPPINGS, delegate(MAPPING tempMapping) 
        	{
        	  return (tempMapping.NAME == mappingName && tempMapping.TYPE == mappingType);
        	});
        	return M;
        }
        
        public DATAOBJECT getDataObject(string dataobjectDataSourceName, string dataobjectDataObjectName) {
        	DATASOURCE DS = Array.Find(this.DATASOURCES, delegate(DATASOURCE tempDataSource) {
        	                           	return tempDataSource.NAME == dataobjectDataSourceName;
        	                           });
        	
        	DATAOBJECT D = Array.Find(DS.DATAOBJECTS, delegate(DATAOBJECT tempDataObject) {
        	                          	return tempDataObject.NAME == dataobjectDataObjectName;
        	                          });
        	return D;
        }
        
        public DIMENSION getDimension(string dimensionName) {
        	DIMENSION DIM = Array.Find(this.CUBE.DIMENSIONS, delegate(DIMENSION tempDimension) {
        	                           	return tempDimension.NAME == dimensionName;
        	                           });
        	return DIM;
        }

        [System.Xml.Serialization.XmlIgnore]
        public List<DATASOURCE> DATASOURCEList { get { return new List<DATASOURCE>(DATASOURCES); } }

        public SOLUTION()
        {
        }
    }
}
