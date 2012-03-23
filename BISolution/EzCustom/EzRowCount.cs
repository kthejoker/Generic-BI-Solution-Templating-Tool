using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.SSIS.EzAPI;

namespace WpfApplication1.EzCustom
{
    [CompID("DtsTransform.RowCount")]
    public class EzRowCount : EzComponent
    {

        public EzRowCount(EzDataFlow dataFlow) : base(dataFlow) { }

        public string VariableName
        {

            get { return (string)m_meta.CustomPropertyCollection["VariableName"].Value; }

            set { m_comp.SetComponentProperty("VariableName", value); ReinitializeMetaData(); }

        }
    }

}
