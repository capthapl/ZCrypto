using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using ZCrypto.BLL.EF;
using ZCrypto.BLL.Model.Api;

namespace ZCrypto.BLL
{
    public class PaprikaApiClient : IDisposable
    {
        private static string BASE_URL = "https://api.coinpaprika.com/";
        private static string DEFAULT_EXCHANGE_ID = "binance";
        RestClient restClient;

        public PaprikaApiClient()
        {
            restClient = new RestClient(BASE_URL);
        }

        public List<API_Coin> GetAllCoins()
        {
            IRestResponse response = restClient.Get(new RestRequest("v1/coins", Method.GET));
            ValidateResponse(response);
            string jsonCoinList = response.Content;
            return JsonConvert.DeserializeObject<List<API_Coin>>(jsonCoinList);
        }

        public API_Market_Price GetMarketPrice(string coinId)
        {
            using zcryptoContext dbCtx = new zcryptoContext();
            if (DataConsistentController.IsCoinBlacklisted(coinId))
                return null;

            System.Diagnostics.Debug.WriteLine(coinId);
            IRestResponse response = restClient.Get(new RestRequest($"v1/coins/{coinId}/markets?quotes=PLN,EUR", Method.GET));
            try
            {
                ValidateResponse(response);
            }catch(Exception)
            {
                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    dbCtx.CoinBlacklistIntegrations.Add(new CoinBlacklistIntegration()
                    {
                        CoinId = coinId,
                        Reason = "Not found by markets endpoint"
                    });
                    dbCtx.SaveChanges();
                    return null;
                }
                else
                {
                    throw;
                }
            }
            string jsonCoinList = response.Content;
            API_Market_Price foundMarketPrice = JsonConvert.DeserializeObject<List<API_Market_Price>>(jsonCoinList)
                .Where(x => x.exchange_id.Equals(DEFAULT_EXCHANGE_ID)).FirstOrDefault();
            return foundMarketPrice;
        }

        private int List<T>(string jsonCoinList)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        private void ValidateResponse(IRestResponse response)
        {
            if (!response.IsSuccessful)
                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new Exception("PaprikaCoin api error: " + response.ErrorMessage);
        }
    }
}