using System;
using System.Collections.Concurrent;
using System.Data;

namespace Simbad.Utils.Domain.Infrastructure
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
            OnCacheInvalidatePartially(id);
        }

        protected virtual void OnCacheInvalidatePartially(int id)
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
                result = base.Get(id);
                Cache.TryAdd(id, result);
            }

            return result;
        }
    }
}
