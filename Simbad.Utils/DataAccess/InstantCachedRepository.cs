using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

using Simbad.Utils.Domain;

namespace Simbad.Utils.DataAccess
{
    public abstract class InstantCachedRepository<TEntity> : RepositoryBase<TEntity> where TEntity : EntityBase, IAggregateRoot
    {
        private readonly object _sync = new object();

        private ICollection<TEntity> _cache;

        protected InstantCachedRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void InvalidateAllCache()
        {
            lock (_sync)
            {
                _cache = null;
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
                EnsureCache(isolationLevel);

                var entity = _cache.FirstOrDefault(e => e.Id == id);

                return entity;
            }
        }

        public ICollection<TEntity> GetAll(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            lock (_sync)
            {
                EnsureCache(isolationLevel);

                return _cache;
            }
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
                    }
                }
            }
        }

        private ICollection<TEntity> InitializeCache(IsolationLevel isolationLevel)
        {
            var result = CallInTransaction(
                isolationLevel,
                (connection, transaction) =>
                    {
                        var toReturn = InitializeCacheInternal(connection, transaction, isolationLevel);
                        transaction.Commit();
                        return toReturn;
                    });

            return result;
        }

        protected abstract ICollection<TEntity> InitializeCacheInternal(IDbConnection connection, IDbTransaction transaction, IsolationLevel isolationLevel);
    }
}