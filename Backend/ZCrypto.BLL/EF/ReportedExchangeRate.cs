using System;
using System.Collections.Generic;

#nullable disable

namespace ZCrypto.BLL.EF
{
    public partial class ReportedExchangeRate
    {
        public int Id { get; set; }
        public int BuyId { get; set; }
        public double PlnExchange { get; set; }
        public DateTime ApiUpdateTime { get; set; }
        public double ExchangeDiff { get; set; }

        public virtual Buy Buy { get; set; }
    }
}
