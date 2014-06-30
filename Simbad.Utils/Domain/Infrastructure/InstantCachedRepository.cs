using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace Simbad.Utils.Domain.Infrastructure
{
    public abstract class InstantCachedRepository<TEntity> : RepositoryBase<TEntity> where TEntity : EntityBase, IAggregateRoot
    {
        private readonly object _sync = new object();

        private TEntity[] _cache;

        private IDictionary<int, TEntity> _map;

        protected InstantCachedRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void InvalidateAllCache()
        {
            lock (_sync)
            {
                _cache = null;
                _map = null;
            }

            OnCacheInvalidatedFully();
        }

        protected virtual void OnCacheInvalidatedFully()
        {
        }

        public override TEntity Get(int id, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            lock (_sync)
            {
                var map = GetCacheAsMap(isolationLevel);

                if (map.ContainsKey(id))
                {
                    return map[id];
                }

                return null;
            }
        }

        public TEntity[] GetAll(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            lock (_sync)
            {
                return GetCacheAsArray(isolationLevel);
            }
        }

        protected TEntity[] GetCacheAsArray(IsolationLevel isolationLevel)
        {
            EnsureCache(isolationLevel);
            return _cache.ToArray();
        }

        protected IDictionary<int, TEntity> GetCacheAsMap(IsolationLevel isolationLevel)
        {
            EnsureCache(isolationLevel);
            return _map;
        }

        private void EnsureCache(IsolationLevel isolationLevel)
        {
            if (_cache == null)
            {
                lock (_sync)
                {
                    if (_cache == null)
                    {
                        var c = InitializeCache(isolationLevel);
                        Thread.MemoryBarrier();
                        _cache = c;
                        _map = c.ToDictionary(i => i.Id, i => i);
                    }
                }
            }
        }

        protected abstract TEntity[] InitializeCache(IsolationLevel isolationLevel);
    }
}