using System;
using System.Collections.Generic;

#nullable disable

namespace ZCrypto.BLL.EF
{
    public partial class CoinBlacklistIntegration
    {
        public int Id { get; set; }
        public string CoinId { get; set; }
        public string Reason { get; set; }

        public virtual Coin Coin { get; set; }
    }
}
