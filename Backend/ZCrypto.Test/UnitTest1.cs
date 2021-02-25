using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ZCrypto.BLL;
using ZCrypto.BLL.EF;
using ZCrypto.BLL.Model.Api;

namespace ZCrypto.Test
{
    public class Tests
    {
        [Test]
        public void GetAllCoins()
        {
            DataConsistentController.SynchronizeCoins();

            using (PaprikaApiClient client = new PaprikaApiClient())
            {
                Assert.DoesNotThrow(() =>
                {
                    List<API_Coin> coins = client.GetAllCoins();
                    Assert.IsTrue(coins.Count > 0);
                });
            }
        }

        [Test]
        public void GetMarketPrices()
        {
            using (zcryptoContext ctx = new zcryptoContext())
            {
                List<Coin> coins = ctx.Coins.Where(x => x.IsActive).ToList();
                using (PaprikaApiClient client = new PaprikaApiClient())
                {
                    foreach (Coin c in coins)
                    {
                        API_Market_Price marketPrice = client.GetMarketPrice(c.Id);
                        if (marketPrice != null)
                        {
                            Assert.IsTrue(marketPrice.quotes.EUR != null);
                            Assert.IsTrue(marketPrice.quotes.EUR.price != 0);
                            Assert.IsTrue(marketPrice.quotes.PLN != null);
                            Assert.IsTrue(marketPrice.quotes.PLN.price != 0);
                        }
                    }
                }
            }
        }

        [Test]
        public void IsCointBlacklisted()
        {
            zcryptoContext ctx = new zcryptoContext();
            foreach (Coin c in ctx.Coins)
            {
                Assert.DoesNotThrow(() => { DataConsistentController.IsCoinBlacklisted(c.Id); });
            }
        }

        [Test]
        public void GetActiveBuysJustActive()
        {
            foreach (Buy b in DataConsistentController.GetActiveBuys())
            {
                Assert.IsTrue(b.Active);
            }
        }
    }
}