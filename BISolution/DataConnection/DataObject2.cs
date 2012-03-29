/*
 * Created by SharpDevelop.
 * User: Kyle
 * Date: 03/20/2012
 * Time: 17:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using WpfApplication1.DataSource;
using WpfApplication1.Static;
using WpfApplication1.Cube;

namespace WpfApplication1.DataConnection
{
	/// <summary>
	/// Description of DataObject2.
	/// </summary>
	public class DataObject2
		    
	{
		
		private string _DATABASELAYER;
		
		[System.Xml.Serialization.XmlIgnore]
        public CUBE cube;
		
		[System.Xml.Serialization.XmlIgnore]
        private WpfApplication1.DataConnection.DataConnection DataConnection_field;
        
        [System.Xml.Serialization.XmlAttribute("NAME")]
        public string NAME { get; set; }
		
		public DATASOURCE ds;
        public string OBJECTTYPE;
        public string DATABASELAYER {get {return _DATABASELAYER;} set {
        		if (this.DataConnection_field != null) {
        		this.DataConnection_field = this.ds.s.getCurrentTier().getDatabaseByLayer(value).DataConnection;
        		}
        		this._DATABASELAYER = value;
        	}}

        [System.Xml.Serialization.XmlIgnore]
        public WpfApplication1.DataConnection.DataConnection dc
        {
            get
            {
                if (this.DataConnection_field == null)
                {
                	if (this.OBJECTTYPE == "DIMENSION") {
                    	this.DataConnection_field = this.cube.s.getCurrentTier().getDatabaseByLayer(this.DATABASELAYER).DataConnection;
                	}
                	else {
                		this.DataConnection_field = this.ds.s.getCurrentTier().getDatabaseByLayer(this.DATABASELAYER).DataConnection;
                	}
                }
                return this.DataConnection_field;
            }
            set { this.DataConnection_field = value; }
        }
        
		
		public List<DataConnection.Column> getColumns() 
        {

              List<DataConnection.Column> columns = new List<DataConnection.Column>();
			return columns;
                           
        }

		public void createTable(string DatabaseLayer = "", bool dropExisting = false)
        {

        	if (DatabaseLayer.Equals("")) { DatabaseLayer = this.DATABASELAYER;}
        	string tableName;
        	if (DatabaseLayer.Equals("DIM")) {
           		tableName = Prefixes.Prefix[DatabaseLayer] + "_" + this.NAME;
        	   }
        	    else {
        	    	tableName = Prefixes.Prefix[DatabaseLayer] + this.ds.NAME + "_" + this.NAME;
        	    }
            string fixedColumns = Columns.Column[DatabaseLayer];
            string tableColumns = "";

            if (!DatabaseLayer.Equals("MATCH")) {
            	foreach (WpfApplication1.DataConnection.DataConnection.Column c in this.getColumns())
            	{
                	tableColumns = tableColumns + c.convertToSQL() + ", ";
            	}
           	
            }           
            string columnInfo = tableColumns + fixedColumns;

            if (dropExisting)
            {
                this.dc.dropTable(tableName);
            }
            this.dc.createTable(tableName, columnInfo);

        }   
		   
		   
	}
}
