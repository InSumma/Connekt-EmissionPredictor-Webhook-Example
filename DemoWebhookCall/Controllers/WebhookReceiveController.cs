using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoWebhookCall.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebhookCall.Controllers
{
    [Route("api/[controller]")]
    public class WebhookReceiveController : Controller
    {

        // POST api/values
        [HttpPost]
        public void Receive([FromBody] List<EmissionType> value)
        {
            // Check security header
            if (Request.Headers["x-secretKey"].Equals("YourKey"))
            {
                // Check if 2legs calculations are updated
                if (value.Where(v => v.Type == "2Legs").Any())
                {
                    // 2leg updated code
                }
                else if (value.Where(v => v.Type == "3Legs").Any())
                {
                    // 3legs updated code
                }
            }
        }

    }
}
