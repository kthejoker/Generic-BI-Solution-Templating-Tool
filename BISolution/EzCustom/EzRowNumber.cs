using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.SSIS.EzAPI;

namespace WpfApplication1.EzCustom
{
    [CompID("Konesans.Dts.Pipeline.RowNumberTransform.RowNumberTransform, Konesans.Dts.Pipeline.RowNumberTransform, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b2ab4a111192992b")]
    public class EzRowNumber : EzComponent
    {

        public EzRowNumber(EzDataFlow dataFlow) : base(dataFlow) { }

    }

}
