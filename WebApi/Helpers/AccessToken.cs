using System;

namespace WebApi.Helpers
{
    public class AccessToken
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
