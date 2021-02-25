using System;
using System.Collections.Generic;

#nullable disable

namespace ZCrypto.BLL.EF
{
    public partial class Coin
    {
        public Coin()
        {
            Buys = new HashSet<Buy>();
            CoinBlacklistIntegrations = new HashSet<CoinBlacklistIntegration>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Rank { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Buy> Buys { get; set; }
        public virtual ICollection<CoinBlacklistIntegration> CoinBlacklistIntegrations { get; set; }
    }
}
