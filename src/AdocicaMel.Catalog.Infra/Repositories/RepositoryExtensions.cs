using MongoDB.Driver;
using System.Linq;

namespace AdocicaMel.Catalog.Infra.Repositories
{
    public static class RepositoryExtensions
    {
        public static IFindFluent<T, T> Paginate<T>(this IFindFluent<T, T> query, int pageNumber, int pageSize)
        {
            return query
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize);
        }
    }
}
