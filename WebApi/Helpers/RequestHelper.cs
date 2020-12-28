using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using WebApi.Core.Extensions;

namespace WebApi.Helpers
{
    public static class RequestHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Initialize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static int GetUserId()
        {
            string bearerToken = GetBearerToken();
            return bearerToken.IsNullOrEmpty() ? 0 : JwtSecurityTokenHelper.GetUserId(bearerToken);
        }

        public static string GetBearerToken()
        {
            return GetTokenAuthorizationHeaderValue("bearer");
        }

        public static string GetBasicToken()
        {
            return GetTokenAuthorizationHeaderValue("basic");
        }

        private static string GetTokenAuthorizationHeaderValue(string tokenType)
        {
            var requestPath = _httpContextAccessor?.HttpContext?.Request?.Path;

            if (requestPath != null && requestPath.Value.Value.Contains("/api/v"))
            {
                if (_httpContextAccessor.HttpContext.Request.Method == "OPTIONS")
                    return "";

                string authorizationHeaderValue = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

                if (authorizationHeaderValue == null)
                    return "";

                var headerValues = authorizationHeaderValue.Split(' ');

                if (headerValues[0].ToLower() != tokenType || headerValues.Length != 2)
                    return "";

                return headerValues[1];
            }

            return "";
        }
    }
}
