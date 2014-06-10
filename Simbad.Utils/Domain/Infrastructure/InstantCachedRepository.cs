using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace Simbad.Utils.Domain.Infrastructure
{
    public abstract class InstantCachedRepository<TEntity> : RepositoryBase<TEntity> where TEntity : EntityBase, IAggregateRoot
    {
        private readonly object _sync = new object();

        private IList<TEntity> _cache;

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
                var map = GetCacheAsMap();

                if (map.ContainsKey(id))
                {
                    return map[id];
                }

                return null;
            }
        }

        public IList<TEntity> GetAll()
        {
            lock (_sync)
            {
                return new List<TEntity>(GetCacheAsList());
            }
        }

        protected IList<TEntity> GetCacheAsList()
        {
            EnsureCache();
            return _cache;
        }

        protected IDictionary<int, TEntity> GetCacheAsMap()
        {
            EnsureCache();
            return _map;
        }

        private void EnsureCache()
        {
            if (_cache == null)
            {
                lock (_sync)
                {
                    if (_cache == null)
                    {
                        var c = InitializeCache();
                        Thread.MemoryBarrier();
                        _cache = c;
                        _map = c.ToDictionary(i => i.Id, i => i);
                    }
                }
            }
        }

        protected abstract IList<TEntity> InitializeCache();
    }
}