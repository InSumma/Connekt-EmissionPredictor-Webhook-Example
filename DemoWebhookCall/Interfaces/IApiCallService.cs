using DemoWebhookCall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoWebhookCall.Interfaces
{
    public interface IApiCallService
    {
        Task<AccesModel> GetAccessTokenWithCredentials();

        Task<AccesModel> GetAccessTokenWithRefreshToken(string refreshToken);
        Task<ReturnModel> CallSubscribtion(string accessToken);

        Task<ReturnModel> CallTest(string accessToken);
    }
}
