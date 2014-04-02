using System;
using System.Collections.Concurrent;

namespace Simbad.Utils.Domain.Infrastructure
{
    public abstract class LazyCachedRepository<TEntity>: RepositoryBase<TEntity> where TEntity : EntityBase, IAggregateRoot
    {
        public event EventHandler CacheInvalidatedFully;

        public event EventHandler<CacheInvalidatedPartiallyEventArgs> CacheInvalidatedPartially;

        protected readonly ConcurrentDictionary<int, TEntity> Cache = new ConcurrentDictionary<int, TEntity>();

        protected LazyCachedRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void InvalidateCache(int id, bool shouldRaiseEvent = true)
        {
            TEntity result;
            Cache.TryRemove(id, out result);

            if (shouldRaiseEvent)
            {
                OnCacheInvalidatePartially(id);
            }
        }

        private void OnCacheInvalidatePartially(int id)
        {
            if (CacheInvalidatedPartially != null)
            {
                CacheInvalidatedPartially(this, new CacheInvalidatedPartiallyEventArgs(id));
            }
        }

        public void InvalidateAllCache(bool shouldRaiseEvent = true)
        {
            Cache.Clear();

            if (shouldRaiseEvent)
            {
                OnCacheInvalidatedFully();
            }
        }

        private void OnCacheInvalidatedFully()
        {
            if (CacheInvalidatedFully != null)
            {
                CacheInvalidatedFully(this, new EventArgs());
            }
        }

        public override TEntity Get(int id)
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
