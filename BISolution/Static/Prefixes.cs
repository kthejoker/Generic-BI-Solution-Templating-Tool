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

        public static Dictionary<string, string> Column = new Dictionary<string, string> { 
            { "STAGE", "MatchKey int NOT NULL, Checksum int NOT NULL" }, 
            { "PSA", "MatchKey int NOT NULL, Checksum int NOT NULL, CreatedDate datetime NOT NULL, ActionCode varchar(10) NOT NULL, ActiveFlag varchar(1) NOT NULL" },   
            { "MATCH", "[NaturalKey] [nvarchar](700) NOT NULL, [NaturalKeyColumnName] [nvarchar](400) NOT NULL, [MatchKey] [int] NOT NULL, [DataSetID] [int] NULL" },
            { "DIM", "MatchKey int NOT NULL"}
        };

   
   }

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
