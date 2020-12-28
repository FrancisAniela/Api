using System;
using WebApi.Core.Models;
using WebApi.Core.Repositories;

namespace WebApi.Core.Services.AppConfigs
{
    public class AppConfigService : IAppConfigService
    {
        IWebApiRepository<AppConfig> _appConfigRepository;

        public AppConfigService(IWebApiRepository<AppConfig> appConfigRepository)
        {
            _appConfigRepository = appConfigRepository;
        }


        public int GetJwtTokenMinutesLifetime()
        {
            return int.Parse(GetConfig("JwtTokenMinutesLifetime"));
        }

  
        private string GetConfig(string configName)
        {
            AppConfig config = _appConfigRepository.Get(x => x.ConfigName == configName);

            if (config == null)
                throw new ApplicationException($"AppConfig {configName} doesn't exists");

            return config?.ConfigValue;
        }
    }
}
