using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebApi.Core.Models;
using WebApi.Core.Services.ClientApplications;

namespace WebApi.Middleware
{
    public class BasicApplicationClientAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IClientApplicationService _clientApplicationService;
        private readonly AppSettings _appSettings;

        public BasicApplicationClientAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IClientApplicationService clientApplicationService,
            IOptions<AppSettings> appSettings)
            : base(options, logger, encoder, clock)
        {
            _clientApplicationService = clientApplicationService;
            _appSettings = appSettings.Value;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            ClientApplicationDto clientApplication = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];
                clientApplication = await _clientApplicationService.Authenticate(username, password);
            }
            catch (Exception exc)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (clientApplication == null)
                return AuthenticateResult.Fail("Invalid Username or Password");

            if (!clientApplication.IsActive)
                return AuthenticateResult.Fail("Client not active");

            var claims = new[] {
                new Claim(type:ClaimTypes.NameIdentifier, clientApplication.Id.ToString()),
                new Claim(type: ClaimTypes.Name, clientApplication.ClientName),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
