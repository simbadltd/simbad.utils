using System.Collections.Concurrent;
using System.Data;

using Simbad.Utils.Domain;

namespace Simbad.Utils.DataAccess
{
    public abstract class LazyCachedRepository<TEntity> : RepositoryBase<TEntity> where TEntity : EntityBase, IAggregateRoot
    {
        protected readonly ConcurrentDictionary<int, TEntity> Cache = new ConcurrentDictionary<int, TEntity>();

        protected LazyCachedRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void InvalidateCache(int id)
        {
            TEntity result;
            Cache.TryRemove(id, out result);
            OnCacheInvalidatedPartially(id);
        }

        protected virtual void OnCacheInvalidatedPartially(int id)
        {
        }

        public void InvalidateAllCache()
        {
            Cache.Clear();
            OnCacheInvalidatedFully();
        }

        protected virtual void OnCacheInvalidatedFully()
        {
        }

        public override TEntity Get(int id, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            TEntity result;

            if (!Cache.TryGetValue(id, out result))
            {
                result = base.Get(id, isolationLevel);
                Cache.TryAdd(id, result);
            }

            return result;
        }
    }
}
