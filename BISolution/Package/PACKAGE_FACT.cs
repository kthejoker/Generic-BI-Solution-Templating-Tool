using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.DataSource;
using WpfApplication1.Mappings;
using Microsoft.SqlServer.SSIS.EzAPI;
using WpfApplication1.EzCustom;

namespace WpfApplication1.Package
{
	class PACKAGE_FACT : PACKAGE
    {
		
		
		public PACKAGE_FACT(MAPPING m) : base(m) {
			this.packageType = "FACT";
			
			this.Name = String.Format("Fact - {0}", this.m.NAME);
			
			  this.addConnection("Source", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("PSA").NAME);
            this.addConnection("RUN", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("RUN").NAME);
            this.addConnection("MATCH", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("MATCH").NAME);
            this.addConnection("META", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("META").NAME);
            this.addConnection("DIM", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("DIM").NAME);
            this.addConnection("FACT", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("FACT").NAME);
            
            
            this.addVariable("Audit::RowsMatched");
            this.addVariable("Audit::RowsInserted");
            this.addVariable("Audit::RowsUpdated");
            this.addVariable("Audit::RowsDeleted");
            
            EzExecuteSQLTask DeleteFactRows = new EzExecuteSQLTask(this);
            DeleteFactRows.Name = "Delete Fact Rows";
            //TODO correct query
            DeleteFactRows.SqlStatementSource = "select 1";
            DeleteFactRows.Connection = this.Conns["FACT"];

            //TODO foreach unionable source object, create data flow and attach
            EzDataFlow InsertFacts = new FactDataFlow(this, this, this.m.SOURCEOBJECTS[0]);
          	InsertFacts.Name = "Insert Facts";
            InsertFacts.AttachTo(DeleteFactRows);
			
			this.SaveToFile(this.fileName());
            this.addConfigurations();
            this.addLogging(InsertFacts.Name);
		}
    }
	
	class FactDataFlow : EzDataFlow
    {
    	
    	 public PACKAGE_FACT p;
    	 public SOURCEOBJECT so;
    	
    	public FactDataFlow(EzContainer parent, PACKAGE_FACT p, SOURCEOBJECT so)  : base(parent)
        {
            this.p = p;
            this.so = so;
            
            EzOleDbSource Source = new EzOleDbSource(this);
            Source.Connection = p.Conns["Source"];
			//TODO correct query
            Source.SqlCommand = String.Format("select * from {0} where ActiveFlag = 'Y' and CreatedDate > '01-01-1900'", so.DATAOBJECT.tableName("PSA"));
            Source.Name = p.m.NAME;
            
            EzLookup Lookup_1 = new EzLookup(this);
            Lookup_1.AttachTo(Source);
            
            EzRowCount RowsInserted = new EzRowCount(this);
            RowsInserted.Name = "Rows Inserted";
            RowsInserted.VariableName = "Audit::RowsInserted";
            RowsInserted.AttachTo(Lookup_1);
            
            EzOleDbDestination FactDestination = new EzOleDbDestination(this);
            FactDestination.Name =this.p.tableName();
            FactDestination.AttachTo(RowsInserted);
            FactDestination.Connection = p.Conns["FACT"];
            FactDestination.Table = p.tableName();
            FactDestination.LinkAllInputsToOutputs();
            FactDestination.ReinitializeMetaData();

    	}
	}
	
}
