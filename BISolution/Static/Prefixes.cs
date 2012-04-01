using System;
using System.Collections.Generic;

namespace WpfApplication1.Static
{
    static class Prefixes
    {
        public static Dictionary<string, string> Prefix = new Dictionary<string, string> { 
            { "STAGE", "stg" },
            { "PSA", "psa" },
            { "MATCH", "match" } ,
            {"DIM", "dim"},
            {"FACT", "fact"}
        };

      

    }

    static class Columns
    {

    	//TODO pass these as Column objects
        public static Dictionary<string, List<WpfApplication1.DataConnection.DataConnection.Column>> Column = new Dictionary<string, List<WpfApplication1.DataConnection.DataConnection.Column>> { 
    		{ "STAGE", new List<WpfApplication1.DataConnection.DataConnection.Column> { new Column("MatchKey", "int"), new Column("Checksum", "int") } },
            { "PSA",
 				new List<WpfApplication1.DataConnection.DataConnection.Column> { 
    					new Column("MatchKey", "int"), 
    					new Column("Checksum", "int"),
    					new Column("CreatedDate", "datetime"), 
    					new Column("ActionCode", "varchar(10)"),
						new Column("ActiveFlag", "char(1)")


    			},
    		{ "MATCH", 
    			new List<WpfApplication1.DataConnection.DataConnection.Column> { 
    					new Column("NaturalKey", "nvarchar(700)"), 
    					new Column("NaturalKeyColumnName", "nvarchar(400)"),
    					new Column("MatchKey", "int"), 
    					new Column("DataSetID", "int")


    			}
    				
    		 },
            { "DIM",
    			
    				new List<WpfApplication1.DataConnection.DataConnection.Column> { 
    					new Column("MatchKey", "int")
    				}
    			}
        };

   
   }

    //TODO allow custom types
    static class DimensionAttributeTypes
    {

        public static Dictionary<string, List<KeyValuePair<string, string>>> DimensionAttributeType = new Dictionary<string, List<KeyValuePair<string, string>>>
        {
            {"Name", new List<KeyValuePair<string, string>>{ 
                    new KeyValuePair<string, string>("DataType","varchar"),
                    new KeyValuePair<string, string>("Length","255")

                }
            }
        };
    }
}
