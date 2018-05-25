using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoWebhookCall.Models
{
    public class AccesModel
    {
        public string AccesToken { get; set; }
        public DateTime ExpireDateTimeAccesToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDateTimeRefreshToken { get; set; }

        public string GetAccesToken()
        {
            return AccesToken;
        }

        public string GetRefreshToken()
        {
            return RefreshToken;
        }

        public string GetData()
        {
            return $"AccesToken: {AccesToken}\r\n" +
                   $"\r\nExpireDateTimeAccesToken: {ExpireDateTimeAccesToken} UTC\r\n" +
                   $"\r\nRefreshToken: {RefreshToken}\r\n" +
                   $"\r\nExpireDateTimeRefreshToken: {ExpireDateTimeRefreshToken} UTC\r\n";
        }

    }
}
