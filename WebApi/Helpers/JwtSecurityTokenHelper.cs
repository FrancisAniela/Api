using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApi.Core.Models;

namespace WebApi.Helpers
{
    public static class JwtSecurityTokenHelper
    {
        private static AppSettings _appSettings;

        public static void Initialize(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public static AccessToken GenerateJwtTokenForUser(int id, string name, string lastname, string email, int munitesLifetime, TokenRoleEnum role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);

            AccessToken result = new AccessToken();
            result.ExpiresAt = DateTime.UtcNow.AddMinutes(munitesLifetime);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id.ToString()),
                    new Claim(ClaimTypes.GivenName, name),
                    new Claim(ClaimTypes.Surname, lastname),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, ((int)role).ToString())
                }),
                Expires = result.ExpiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.JwtIssuer,

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            result.Token = tokenHandler.WriteToken(token);

            return result;
        }

        public static AccessToken GenerateJwtTokenForClientApplication(int id, int munitesLifetime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);

            AccessToken result = new AccessToken();
            result.ExpiresAt = DateTime.UtcNow.AddMinutes(munitesLifetime);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id.ToString()),
                    new Claim(ClaimTypes.Role, ((int)TokenRoleEnum.ClientApplication).ToString())
                }),
                Expires = result.ExpiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.JwtIssuer,

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            result.Token = tokenHandler.WriteToken(token);

            return result;
        }

        public static bool ValidateToken(string token, TokenRoleEnum role)
        {
            try
            {
                var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.JwtSecret));
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidIssuer = _appSettings.JwtIssuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);

                var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                int tokenRole = int.Parse(securityToken.Claims.First(claim => claim.Type == "role").Value);

                return tokenRole == (int)role;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public static int GetUserId(string jwtToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == "unique_name");

            if (userIdClaim == null)
                throw new UnauthorizedAccessException();

            return int.Parse(userIdClaim.Value);
        }

        public static TokenRoleEnum GetTokenRole(string jwtToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var roleClaim = token.Claims.FirstOrDefault(x => x.Type == "role");

            if (roleClaim == null)
                throw new UnauthorizedAccessException();

            return (TokenRoleEnum)int.Parse(roleClaim.Value);
        }
    }
}
