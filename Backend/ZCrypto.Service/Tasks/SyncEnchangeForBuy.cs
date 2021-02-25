using System.Collections.Generic;
using ZCrypto.BLL;
using ZCrypto.BLL.EF;
using ZCrypto.BLL.Model.Api;
using ZCrypto.Service.Abstract;

namespace ZCrypto.Service.Tasks
{
    public class SyncEnchangeForBuy : IZCryptoTask
    {
        public void Run()
        {
            using PaprikaApiClient client = new PaprikaApiClient();
            using zcryptoContext ctx = new zcryptoContext();
            List<Buy> activeBuys = DataConsistentController.GetActiveBuys();
            foreach (Buy b in activeBuys)
            {
                API_Market_Price price = client.GetMarketPrice(b.CoinId);
                if (price is not null)
                {
                    ctx.ReportedExchangeRates.Add(new ReportedExchangeRate()
                    {
                        BuyId = b.Id,
                        PlnExchange = price.quotes.PLN.price,
                        ApiUpdateTime = price.last_updated,
                        ExchangeDiff = (price.quotes.PLN.price * (double) b.Count) - (double) (b.ExchangeRatePln * b.Count)
                    });
                    ctx.SaveChanges();
                }
            }
        }
    }
}