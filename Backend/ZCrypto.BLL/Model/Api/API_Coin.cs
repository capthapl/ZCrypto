using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCrypto.BLL.Model.Api
{
    public class API_Coin
    {
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public int rank { get; set; }
        public bool is_new { get; set; }
        public bool is_active { get; set; }
        public string type { get; set; }
    }
}
