using WebApi.Core.Models;

namespace WebApi.Core.Repositories
{
    public interface ILoggerRepository
    {
        void InsertLog(ApplicationLog log);
    }
}
