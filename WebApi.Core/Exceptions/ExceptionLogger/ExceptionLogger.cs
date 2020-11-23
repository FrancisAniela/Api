using Microsoft.Extensions.Options;
using System;
using WebApi.Core.Enums;
using WebApi.Core.Models;
using WebApi.Core.Repositories;

namespace WebApi.Core.Exceptions.ExceptionLogger
{
    public class ExceptionLogger : IExceptionLogger
    {

        ILoggerRepository _loggerRepository;
        AppSettings _appSettings;

        public ExceptionLogger(ILoggerRepository loggerRepository
            , IOptions<AppSettings> appSenttings)
        {
            _loggerRepository = loggerRepository;
            _appSettings = appSenttings.Value;
        }

        public void LogException(Exception exc)
        {
            try
            {
                ApplicationLog log = new ApplicationLog()
                {
                    LogType = LogTypeEnum.Error.ToString(),
                    ApplicationName = _appSettings.ApplicationName,
                    Date = DateTime.Now,
                    Message = exc.Message,
                    FormattedMessage = exc != null ? exc.ToString() : String.Empty
                };

                _loggerRepository.InsertLog(log);
            }
            catch (Exception ex)
            {
            }
        }

    }
}
