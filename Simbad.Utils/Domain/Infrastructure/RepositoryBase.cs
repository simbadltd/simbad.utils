using System.Collections.Generic;
using System.Data;

namespace Simbad.Utils.Domain.Infrastructure
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase, IAggregateRoot
    {
        protected readonly IConnectionFactory ConnectionFactory;

        protected RepositoryBase(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public void Persist(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (item.IsNew)
            {
                Add(item, isolationLevel);
            }
            else
            {
                Update(item, isolationLevel);
            }
        }

        public void Add(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var cn = ConnectionFactory.CreateConnection())
            {
                cn.Open();

                using (var transaction = cn.BeginTransaction(isolationLevel))
                {
                    SetSystemFields(item, cn, transaction);
                    AddInternal(item, cn, transaction);
                    transaction.Commit();
                }
            }
        }

        protected virtual void SetSystemFields(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        protected virtual void AddInternal(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public void Update(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var cn = ConnectionFactory.CreateConnection())
            {
                cn.Open();

                using (var transaction = cn.BeginTransaction(isolationLevel))
                {
                    SetSystemFields(item, cn, transaction);
                    UpdateInternal(item, cn, transaction);
                    transaction.Commit();
                }
            }
        }

        protected virtual void UpdateInternal(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public void Remove(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var cn = ConnectionFactory.CreateConnection())
            {
                cn.Open();

                using (var transaction = cn.BeginTransaction(isolationLevel))
                {
                    RemoveInternal(item, cn, transaction);
                    transaction.Commit();
                }
            }
        }

        protected virtual void RemoveInternal(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public virtual TEntity Get(int id, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var cn = ConnectionFactory.CreateConnection())
            {
                cn.Open();

                using (var transaction = cn.BeginTransaction(isolationLevel))
                {
                    var result = GetInternal(id, cn, transaction);
                    transaction.Commit();

                    return result;
                }
            }
        }

        protected virtual TEntity GetInternal(int id, IDbConnection connection, IDbTransaction transaction)
        {
            return null;
        }
    }
}