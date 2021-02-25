using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCrypto.BLL.Model.Api
{
    public class API_Market_Price
    {
        public string exchange_id { get; set; }
        public string exchange_name { get; set; }
        public string pair { get; set; }
        public string base_currency_id { get; set; }
        public string base_currency_name { get; set; }
        public string quote_currency_id { get; set; }
        public string quote_currency_name { get; set; }
        public string market_url { get; set; }
        public string category { get; set; }
        public string fee_type { get; set; }
        public bool outlier { get; set; }
        public double adjusted_volume_24h_share { get; set; }
        public Quotes quotes { get; set; }
        public string trust_score { get; set; }
        public DateTime last_updated { get; set; }
    }

    public class PLN
    {
        public double price { get; set; }
        public double volume_24h { get; set; }
    }

    public class EUR
    {
        public double price { get; set; }
        public double volume_24h { get; set; }
    }


    public class Quotes
    {
        public PLN PLN { get; set; }
        public PLN EUR { get; set; }

    }

}
