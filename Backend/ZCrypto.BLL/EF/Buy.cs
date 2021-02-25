using System;
using System.Collections.Generic;

#nullable disable

namespace ZCrypto.BLL.EF
{
    public partial class Buy
    {
        public Buy()
        {
            ReportedExchangeRates = new HashSet<ReportedExchangeRate>();
        }

        public int Id { get; set; }
        public string CoinId { get; set; }
        public decimal ExchangeRatePln { get; set; }
        public decimal Count { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool? Active { get; set; }

        public virtual Coin Coin { get; set; }
        public virtual ICollection<ReportedExchangeRate> ReportedExchangeRates { get; set; }
    }
}
