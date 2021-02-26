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
        public IActionResult AddBuy([FromBody] Buy buy,[FromHeader] string securityCode)
        {
            if (securityCode == null || !securityCode.Equals("secret"))
            {
                return Unauthorized();
            }

            try
            {
                using (zcryptoContext ctx = new zcryptoContext())
                {
                    ctx.Buys.Add(buy);
                    ctx.SaveChanges();
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Buy")]
        public IActionResult GetBuy()
        {
            try
            {
                return Ok(DataConsistentController.GetActiveBuysWithReportedExchangeRates().Take(20));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ActiveCoinIds")]
        public IActionResult GetActiveCoinIds()
        {
            try
            {
                return Ok(DataConsistentController.GetActiveCoinsIds());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}