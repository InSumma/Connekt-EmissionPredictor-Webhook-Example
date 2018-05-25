using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoWebhookCall.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebhookCall.Controllers
{
    [Route("api/[controller]")]
    public class WebhookTriggerController : Controller
    {
        private readonly IApiCallService _apiCallService;
        public WebhookTriggerController(IApiCallService apiCallService)
        {
            _apiCallService = apiCallService;
        }

        [HttpGet]
        public async Task Trigger()
        {
            // Get bearer token (signin)
            var accessToken = await _apiCallService.GetAccessTokenWithCredentials();

            // Subscribe to webhook
            var returnModel = await _apiCallService.CallSubscribtion(accessToken.AccesToken);

            // Send test message to receive controller
            returnModel = await _apiCallService.CallTest(accessToken.AccesToken);
        }

    }
}