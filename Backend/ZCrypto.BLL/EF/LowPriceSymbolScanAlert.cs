using System;
using System.Collections.Generic;

#nullable disable

namespace ZCrypto.BLL.EF
{
    public partial class LowPriceSymbolScanAlert
    {
        public int Id { get; set; }
        public int? LowPriceSymbolScanId { get; set; }
        public DateTime? ActiveDuration { get; set; }
        public decimal? PercentLow { get; set; }

        public virtual LowPriceSymbolScan LowPriceSymbolScan { get; set; }
    }
}
