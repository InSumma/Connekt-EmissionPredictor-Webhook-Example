using DemoWebhookCall.Interfaces;
using DemoWebhookCall.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebhookCall.Services
{
    public class ApiCallService : IApiCallService
    {

        private readonly IConfiguration _Config;

        public ApiCallService(IConfiguration Config)
        {
            _Config = Config;
        }

        public async Task<AccesModel> GetAccessTokenWithCredentials()
        {
            using (var client = new HttpClient())
            {
                if (!String.IsNullOrEmpty(_Config["Settings:ShopId"]) && !String.IsNullOrEmpty(_Config["Settings:Password"]))
                {
                    var body = new Dictionary<string, string>
                    {
                        { "id", _Config["Settings:ShopId"]},
                        { "password", _Config["Settings:Password"] }
                    };
                    var httpContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{_Config["Settings:ApiUrl"]}/api/account/token", httpContent);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        JToken result = JObject.Parse(responseString);
                        return MapAccesModel(result);
                    }
                    else
                    {
                        JToken result = JObject.Parse(responseString);
                        var errors = result["errors"].ToString();
                        throw new HttpRequestException($"Something went wrong statuscode: {response.StatusCode} Errors: {errors}");
                    }
                }
                throw new HttpRequestException($"You have to provide a shopId and a password");
            }
        }

        public async Task<AccesModel> GetAccessTokenWithRefreshToken(string refreshToken)
        {
            using (var client = new HttpClient())
            {
                var body = new Dictionary<string, string>
                {
                   { "refreshToken", refreshToken}
                };
                var httpContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_Config["Settings:ApiUrl"]}/api/account/refresh", httpContent);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    JToken result = JObject.Parse(responseString);
                    return MapAccesModel(result);
                }
                else
                {
                    JToken result = JObject.Parse(responseString);
                    var errors = result["errors"].ToString();
                    throw new HttpRequestException($"Something went wrong statuscode: {response.StatusCode} Errors: {errors}");
                }
            }
        }

        public async Task<ReturnModel> CallSubscribtion(string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // This url is the iisexpress url configured in the launchsettings.json in this project
                string returnUrl = _Config["Settings:ReturnUrl"];
                string secretKey = _Config["Settings:SecretKey"];

                var queryString = $"returnUrl={System.Net.WebUtility.UrlEncode(returnUrl)}&secretKey={secretKey}";

                var response = await client.GetAsync($"{_Config["Settings:ApiUrl"]}/api/webhook-subscription/subscribe?" + queryString);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return new ReturnModel() { Result = true };
                }
                else
                {
                    JToken result = JObject.Parse(responseString);
                    var errors = result["errors"].ToString();
                    return new ReturnModel() { Result = false };
                }
            }
        }

        public async Task<ReturnModel> CallTest(string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync($"{_Config["Settings:ApiUrl"]}/api/webhook-subscription/test");
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    JToken result = JObject.Parse(responseString);
                    return new ReturnModel() { Result = true };
                }
                else
                {
                    JToken result = JObject.Parse(responseString);
                    var errors = result["errors"].ToString();
                    throw new HttpRequestException($"Something went wrong statuscode: {response.StatusCode} Errors: {errors}");
                }
            }
        }

        private AccesModel MapAccesModel(JToken token)
        {
            return new AccesModel()
            {
                AccesToken = token["accessToken"].ToString(),
                ExpireDateTimeAccesToken = Convert.ToDateTime(token["expireDateTimeAccesToken"].ToString()),
                RefreshToken = token["refreshToken"].ToString(),
                ExpireDateTimeRefreshToken = Convert.ToDateTime(token["expireDateTimeRefreshToken"].ToString())
            };
        }       
    }
}
