using DapperExtensions;

namespace Simbad.Utils.Orm
{
    public interface ISpecification<TEntity, TId> where TEntity : class, IEntity<TId>, IAggregationRoot
    {
        IPredicate Execute();
    }
}