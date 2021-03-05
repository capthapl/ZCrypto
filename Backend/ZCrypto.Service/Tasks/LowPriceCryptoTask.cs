using System;
using System.Collections.Generic;
using System.Linq;
using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using ZCrypto.BLL.EF;
using ZCrypto.Service.Abstract;

namespace ZCrypto.Service.Tasks
{
    public class LowPriceCryptoTask : IZCryptoTask
    {
        public void Run()
        {
            using zcryptoContext ctx = new zcryptoContext();
            using BinanceClient client = new BinanceClient();

            List<LowPriceSymbolScan> symbolsToScan = ctx.LowPriceSymbolScans.ToList();

            foreach (LowPriceSymbolScan symbol in symbolsToScan)
            {
                List<IBinanceKline> klines = client.Spot.Market.GetKlines(symbol.Symbol, KlineInterval.FiveMinutes, null, null, symbol.LastKlines).Data.ToList();
                decimal goingDown = CheckItsGoingDown(klines, symbol.PercentThreshold, symbol.Symbol);
                if (goingDown>0)
                {
                    List<LowPriceSymbolScanAlert> alerts = ctx.LowPriceSymbolScanAlerts.ToList();
                    if (alerts.Count(x => x.LowPriceSymbolScanId == symbol.Id && x.ActiveDuration >= DateTime.Now) > 0)
                    {
                        foreach (var alert in ctx.LowPriceSymbolScanAlerts.Where(x=> x.LowPriceSymbolScanId == symbol.Id && x.ActiveDuration >= DateTime.Now))
                        {
                            alert.ActiveDuration = DateTime.Now.AddMinutes(15);
                        }
                    }
                    else
                    {
                        ctx.LowPriceSymbolScanAlerts.Add(new LowPriceSymbolScanAlert()
                        {
                            ActiveDuration = DateTime.Now.AddMinutes(15),
                            LowPriceSymbolScanId = symbol.Id,
                            PercentLow = goingDown
                        });
                    }

                    ctx.SaveChanges();
                }
            }
        }
        
        private decimal CheckItsGoingDown(List<IBinanceKline> close,double percentOff,string symbol)
        {
            decimal lastPercent = 0;
            for (int i = close.Count()-1; i >0; i--)
            {
                var P = close.ElementAt(i);
                var Q = close.ElementAt(i-1);

                decimal height = close.Sum(x => (x.Open - x.Close));
                if (height < 0) return -1;
                decimal avgHeight = close.Average(x => x.High);
                decimal percent = height / avgHeight;
                lastPercent = percent;
                Console.WriteLine(symbol + " ~ " +percent);
                if (percent < Convert.ToDecimal(percentOff) / 100)
                {
                    return -1;
                }
            }

            return lastPercent*100;
        }
    }
}