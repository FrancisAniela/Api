namespace WebApi.Core.Repositories
{
    public interface IWebApiRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
    }
}
