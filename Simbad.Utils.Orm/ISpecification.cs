using DapperExtensions;

namespace Simbad.Utils.Orm
{
    public interface ISpecification<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : struct
    {
        IPredicate Execute();
    }
}