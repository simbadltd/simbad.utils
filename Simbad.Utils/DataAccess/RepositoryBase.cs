using System.Data;

using Simbad.Utils.Domain;

namespace Simbad.Utils.DataAccess
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
            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(isolationLevel))
                {
                    SetSystemFields(item, connection, transaction);
                    AddInternal(item, connection, transaction);
                    transaction.Commit();
                }
            }
        }

        protected virtual void SetSystemFields(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public virtual void AddInternal(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public void Update(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(isolationLevel))
                {
                    SetSystemFields(item, connection, transaction);
                    UpdateInternal(item, connection, transaction);
                    transaction.Commit();
                }
            }
        }

        public virtual void UpdateInternal(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public void Remove(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(isolationLevel))
                {
                    RemoveInternal(item, connection, transaction);
                    transaction.Commit();
                }
            }
        }

        public virtual void RemoveInternal(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public virtual TEntity Get(int id, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(isolationLevel))
                {
                    var result = GetInternal(id, connection, transaction);
                    transaction.Commit();

                    return result;
                }
            }
        }

        public virtual TEntity GetInternal(int id, IDbConnection connection, IDbTransaction transaction)
        {
            return null;
        }
    }
}