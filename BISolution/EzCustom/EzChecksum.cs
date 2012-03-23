using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.SSIS.EzAPI;
using Konesans.Dts.Pipeline.ChecksumTransform;

namespace WpfApplication1.EzCustom
{
    [CompID("Konesans.Dts.Pipeline.ChecksumTransform.ChecksumTransform, Konesans.Dts.Pipeline.ChecksumTransform, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b2ab4a111192992b")]
    public class EzChecksum : EzComponent
    {

        public EzChecksum(EzDataFlow dataFlow) : base(dataFlow) { }

    }
}
