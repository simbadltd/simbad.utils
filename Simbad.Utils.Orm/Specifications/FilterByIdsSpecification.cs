using System.Collections.Generic;
using System.Linq;
using DapperExtensions;

namespace Simbad.Utils.Orm.Specifications
{
    public sealed class FilterByIdsSpecification<TEntity, TId> : ISpecification<TEntity, TId>
        where TEntity : class, IEntity<TId>, IAggregationRoot
        where TId : struct
    {
        private readonly ICollection<TId> _ids;

        public FilterByIdsSpecification(TId id)
        {
            _ids = new[] {id};
        }

        public FilterByIdsSpecification(ICollection<TId> ids)
        {
            _ids = ids;
        }

        public IPredicate Execute()
        {
            if (_ids == null || _ids.Count == 0)
            {
                return null;
            }

            if (_ids.Count == 1)
            {
                return Predicates.Field<TEntity>(x => x.Id, Operator.Eq, _ids.Single());
            }

            var predicates =
                _ids.Select(
                    p => Predicates.Field<TEntity>(x => x.Id, Operator.Eq, p) as IPredicate)
                    .ToArray();

            return Predicates.Group(GroupOperator.Or, predicates);
        }
    }
}