using WebApi.Core.Models;
using WebApi.Core.Repositories;

namespace WebApi.Infrastructure.Repositories
{
    public class LoggerRepository : ILoggerRepository
    {
        public void InsertLog(ApplicationLog log)
        {
            using (var context = new WebApiContext())
            {
                context.SaveChanges();
            }
        }
    }
}
