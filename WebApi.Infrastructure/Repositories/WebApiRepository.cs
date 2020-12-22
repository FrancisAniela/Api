using WebApi.Core.Repositories;

namespace WebApi.Infrastructure.Repositories
{
    public class WebApiRepository<TEntity> : BaseRepository<TEntity>, IWebApiRepository<TEntity>
        where TEntity : class
    {
        public WebApiRepository(WebApiContext dbContext) :
            base(dbContext)
        {

        }
    }
}
