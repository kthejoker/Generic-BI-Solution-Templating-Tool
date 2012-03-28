using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.DataSource;
using Microsoft.SqlServer.SSIS.EzAPI;
using Microsoft.SqlServer.Dts;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask;
using WpfApplication1.Cube;
using WpfApplication1.EzCustom;

namespace WpfApplication1.Package
{
    class PACKAGE_DIM : PACKAGE
    {
    	
    	//TODO get Dts.Runtime dll for reference

          public PACKAGE_DIM(DIMENSION d) : base(d)
        {
            this.packageType = "DIM";

            this.Name = this.tableName();

         
            this.addConnection("Source", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("PSA").NAME);
            this.addConnection("RUN", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("RUN").NAME);
            this.addConnection("MATCH", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("MATCH").NAME);
            this.addConnection("META", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("META").NAME);
            this.addConnection("DIM", this.t.SSMS.SERVERNAME, this.t.getDatabaseByLayer("DIM").NAME);

            this.addVariable("Audit::RowsMatched");
            this.addVariable("Audit::RowsInserted");
            this.addVariable("Audit::RowsUpdated");
            this.addVariable("Audit::RowsDeleted");
            

            // Get last run date to compare against active date.
            EzExecuteSQLTask GetLastRunDate = new EzExecuteSQLTask(this);
            GetLastRunDate.Name = "Get Last Run Date";
            //TODO correct query
            //TODO write result to variable
            GetLastRunDate.SqlStatementSource = "select 1";
            GetLastRunDate.Connection = this.Conns["RUN"];

            EzDataFlow UpdateDimension = new DimDataFlow(this, this);
            UpdateDimension.Name = "Update Dimension";
            UpdateDimension.AttachTo(GetLastRunDate);

            this.SaveToFile(this.fileName());
            this.addConfigurations();
            this.addLogging(UpdateDimension.Name);
        }

    }
    
    class DimDataFlow : EzDataFlow
    {
    	
    	 public PACKAGE_DIM p;
    	
    	public DimDataFlow(EzContainer parent, PACKAGE_DIM p)  : base(parent)
        {
            this.p = p;
            
            EzOleDbSource Source = new EzOleDbSource(this);
            Source.Connection = p.Conns["Source"];
            //TODO build custom query from dimension object
            Source.SqlCommand = "select * from psa_Nucleus_Users where ActiveFlag = 'Y' and CreatedDate > '01-01-1900'";
            //TODO get name from mapping object
            Source.Name = "psa_Nucleus_Users";
            
            EzDerivedColumn DeriveAttributes = new EzDerivedColumn(this);
            DeriveAttributes.AttachTo(Source);
            DeriveAttributes.Name = "Derive Attributes";
            //TODO for each mapping column add attribute expression
            
            EzConditionalSplit ActionCode = new EzConditionalSplit(this);
            ActionCode.AttachTo(DeriveAttributes);
            EzConditionalSplit["case1"] = "ActionCode == 'UPDATE'";
            EzConditionalSplit["case2"] = "ActionCode == 'INSERT'";
            
            RowsMatched = new EzRowCount(this);
            RowsMatched.Name = "Rows Matched";
            RowsMatched.VariableName = "Audit::RowsMatched";
            RowsMatched.AttachTo(ActionCode, 0, 0);
            
            EzRowCount RowsInserted = new EzRowCount(this);
            RowsInserted.Name = "Rows Inserted";
            RowsInserted.VariableName = "Audit::RowsInserted";
            RowsInserted.AttachTo(ActionCode, 1, 0);
            
            EzOleDbDestination InsertedDestination = new EzOleDbDestination(this);
            InsertedDestination.Name =String.Format("{0} - New Records",  this.p.tableName());
            InsertedDestination.AttachTo(RowsInserted);
            InsertedDestination.Connection = p.Conns["DIM"];
            InsertedDestination.Table = p.tableName();
            InsertedDestination.LinkAllInputsToOutputs();
            InsertedDestination.ReinitializeMetaData();

            
//              MAPPING[] dimensionMappings = Array.FindAll(this.SOLUTIONS.MAPPINGS, delegate(MAPPING tempMapping)
//            {
//                return tempMapping.TYPE == "Dimension";
//            });
//           
//            
//            foreach (MAPPING m in dimensionMappings) {
//            	m.
//            }
    	}
	}
}
