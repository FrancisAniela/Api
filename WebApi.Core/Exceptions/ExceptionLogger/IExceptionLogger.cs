using System;

namespace WebApi.Core.Exceptions.ExceptionLogger
{
    public interface IExceptionLogger
    {
        void LogException(Exception exc);
    }
}
