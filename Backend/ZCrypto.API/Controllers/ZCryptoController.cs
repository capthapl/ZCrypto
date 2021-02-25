using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ZCrypto.BLL;
using ZCrypto.BLL.EF;

namespace ZCrypto.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZCryptoController : ControllerBase
    {
        [HttpPost("AddBuy")]
        public IActionResult AddBuy([FromBody] Buy buy)
        {
            using (zcryptoContext ctx = new zcryptoContext())
            {
                ctx.Buys.Add(buy);
                ctx.SaveChanges();
            }

            return Ok();
        }

        [HttpGet("Buy")]
        public IActionResult GetBuy()
        {
            return Ok(DataConsistentController.GetActiveBuysWithReportedExchangeRates().Take(20));
        }
        
    }
}