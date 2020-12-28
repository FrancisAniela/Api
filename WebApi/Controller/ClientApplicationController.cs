using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using WebApi.Core.Services.AppConfigs;
using WebApi.Core.Services.ClientApplications;
using WebApi.Errors;
using WebApi.Helpers;

namespace WebApi.Controller
{   
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ProducesErrorResponseType(typeof(void))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorApiResponse), StatusCodes.Status500InternalServerError)]
    [ApiController]
    public class ClientApplicationController : ControllerBase
    {
        private IClientApplicationService _clientApplicationService;
        private IAppConfigService _appConfirService;

        public ClientApplicationController(IClientApplicationService clientApplicationService
            , IAppConfigService appConfirService)
        {
            _clientApplicationService = clientApplicationService;
            _appConfirService = appConfirService;
        }

        [Authorize(Policy = "BasicClientAuthentication")]
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(AccessToken), StatusCodes.Status200OK)]
        public IActionResult Authenticate()
        {
            var credentialBytes = Convert.FromBase64String(RequestHelper.GetBasicToken());
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);

            var clientName = credentials[0];
            var clientSecret = credentials[1];

            ClientApplicationDto clientApplication = _clientApplicationService.Authenticate(clientName, clientSecret).Result;

            if (clientApplication == null)
                throw new UnauthorizedAccessException("Invalid client");

            if (!clientApplication.IsActive)
                throw new UnauthorizedAccessException("Client is not active");

            AccessToken newToken = JwtSecurityTokenHelper.GenerateJwtTokenForClientApplication(clientApplication.Id, _appConfirService.GetJwtTokenMinutesLifetime());
            return Ok(newToken);
        }

    }
}
