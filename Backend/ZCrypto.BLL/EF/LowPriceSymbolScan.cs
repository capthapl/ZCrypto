using System;
using System.Collections.Generic;

#nullable disable

namespace ZCrypto.BLL.EF
{
    public partial class LowPriceSymbolScan
    {
        public LowPriceSymbolScan()
        {
            LowPriceSymbolScanAlerts = new HashSet<LowPriceSymbolScanAlert>();
        }

        public int Id { get; set; }
        public string Symbol { get; set; }
        public double PercentThreshold { get; set; }
        public int LastKlines { get; set; }

        public virtual ICollection<LowPriceSymbolScanAlert> LowPriceSymbolScanAlerts { get; set; }
    }
}
