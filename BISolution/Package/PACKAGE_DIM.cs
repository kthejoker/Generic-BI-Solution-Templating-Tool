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
using WpfApplication1.Mappings;

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
            this.addVariable("Audit::LastRunDate");
            

            // Get last run date to compare against active date.
            EzExecuteSQLTask GetLastRunDate = new EzExecuteSQLTask(this);
            GetLastRunDate.Name = "Get Last Run Date";
            //TODO correct query
            //TODO write result to variable
            GetLastRunDate.SqlStatementSource = "select 1";
            GetLastRunDate.Connection = this.Conns["RUN"];

            //TODO foreach unionable source object, create data flow and attach
            EzDataFlow UpdateDimension = new DimDataFlow(this, this, this.dim.MAPPING.SOURCEOBJECTS[0]);
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
    	 public SOURCEOBJECT so;
    	
    	public DimDataFlow(EzContainer parent, PACKAGE_DIM p, SOURCEOBJECT so)  : base(parent)
        {
            this.p = p;
            this.so = so;
            
            EzOleDbSource Source = new EzOleDbSource(this);
            Source.Connection = p.Conns["Source"];
            

            Source.SqlCommand = String.Format("select * from {0} where ActiveFlag = 'Y' and CreatedDate > '01-01-1900'", so.DATAOBJECT.tableName("PSA"));
            Source.Name = so.DATAOBJECT.tableName("PSA");
            
            EzDerivedColumn DeriveAttributes = new EzDerivedColumn(this);
            DeriveAttributes.AttachTo(Source);
            DeriveAttributes.Name = "Derive Attributes";
            //TODO for each mapping column add attribute expression
            foreach (MAPPINGCOLUMN mappingColumn in so.MAPPINGCOLUMNS) {
            	if (mappingColumn.ATTRIBUTE == null) {
            		mappingColumn.ATTRIBUTE = mappingColumn.DATACOLUMN;
            	}
            	//TODO based on attribute type determine what sort of SSIS-ifying the data column needs
               DeriveAttributes.Expression[mappingColumn.ATTRIBUTE] = "(DT_STR,150,1252)\"" + mappingColumn.DATACOLUMN + "\"";
            }
            
            EzConditionalSplit ActionCode = new EzConditionalSplit(this);
            ActionCode.AttachTo(DeriveAttributes);
            ActionCode.Condition["case1"] = "ActionCode == 'UPDATE'";
            ActionCode.Condition["case2"] = "ActionCode == 'INSERT'";
            
            EzRowCount RowsMatched = new EzRowCount(this);
            RowsMatched.Name = "Rows Matched";
            RowsMatched.VariableName = "Audit::RowsMatched";
            RowsMatched.AttachTo(ActionCode, 0, 0);
            
            EzOleDbCommand UpdateDimension = new EzOleDbCommand(this);
            UpdateDimension.AttachTo(RowsMatched);
            
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

    	}
	}
}
