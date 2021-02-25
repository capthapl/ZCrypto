using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZCrypto.BLL.EF;
using ZCrypto.BLL.Model.Api;

namespace ZCrypto.BLL
{
    public static class DataConsistentController
    {
        public static void SynchronizeCoins()
        {
            using (PaprikaApiClient client = new PaprikaApiClient())
            {
                foreach (API_Coin coin in client.GetAllCoins())
                {
                    if (!IsCoinBlacklisted(coin.id))
                        UpsertCoin(coin);
                }
            }
        }

        public static bool IsCoinBlacklisted(string coinId)
        {
            using zcryptoContext dbCtx = new zcryptoContext();
            return dbCtx.CoinBlacklistIntegrations.Any(x => x.CoinId.Equals(coinId));
        }

        public static void UpsertCoin(API_Coin coin)
        {
            using (zcryptoContext ctx = new zcryptoContext())
            {
                Coin foundCoin = ctx.Coins.Where(x => x.Id.Equals(coin.id)).FirstOrDefault();
                if (foundCoin is null)
                {
                    ctx.Coins.Add(new Coin()
                    {
                        Id = coin.id,
                        IsActive = coin.is_active,
                        IsNew = coin.is_new,
                        Name = coin.name,
                        Rank = coin.rank,
                        Symbol = coin.symbol,
                        Type = coin.type
                    });
                }
                else
                {
                    foundCoin.IsActive = coin.is_active;
                    foundCoin.IsNew = coin.is_new;
                    foundCoin.Name = coin.name;
                    foundCoin.Rank = coin.rank;
                    foundCoin.Symbol = coin.symbol;
                    foundCoin.Type = coin.type;
                }

                ctx.SaveChanges();
            }
        }

        public static List<Buy> GetActiveBuys()
        {
            using zcryptoContext ctx = new zcryptoContext();
            return ctx.Buys.Where(x => x.Active.HasValue && x.Active.Value).ToList();
        }

        public static List<Buy> GetActiveBuysWithReportedExchangeRates()
        {
            using zcryptoContext ctx = new zcryptoContext();
            return ctx.Buys.Where(x => x.Active.HasValue && x.Active.Value)
                .Include(x=>x.Coin)
                .Include(x => x.ReportedExchangeRates.OrderByDescending(c=>c.Id).Take(1)).ToList().OrderBy(x=>x.Id).ToList();
        }
    }
}