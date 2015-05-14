using System;
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

        public int Persist(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (item.IsNew)
            {
                return Add(item, isolationLevel);
            }
            else
            {
                Update(item, isolationLevel);
                return item.Id;
            }
        }

        public int Add(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var result = CallInTransaction(
                isolationLevel,
                (connection, transaction) =>
                    {
                        SetSystemFields(item, connection, transaction);
                        var toReturn = AddInternal(item, connection, transaction);
                        transaction.Commit();
                        return toReturn;
                    });

            return result;
        }

        protected virtual void SetSystemFields(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public abstract int AddInternal(TEntity item, IDbConnection connection, IDbTransaction transaction);

        public void Update(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            CallInTransaction(
                isolationLevel,
                (connection, transaction) =>
                    {
                        SetSystemFields(item, connection, transaction);
                        UpdateInternal(item, connection, transaction);
                        transaction.Commit();
                    });
        }

        public abstract void UpdateInternal(TEntity item, IDbConnection connection, IDbTransaction transaction);

        public void Remove(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            CallInTransaction(
                isolationLevel,
                (connection, transaction) =>
                    {
                        RemoveInternal(item, connection, transaction);
                        transaction.Commit();
                    });
        }

        public abstract void RemoveInternal(TEntity item, IDbConnection connection, IDbTransaction transaction);

        public TEntity Get(int id, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var result = CallInTransaction(
                isolationLevel,
                (connection, transaction) =>
                    {
                        var toReturn = GetInternal(id, connection, transaction);
                        Fill(toReturn, connection, transaction);
                        transaction.Commit();
                        return toReturn;
                    });

            return result;
        }

        public virtual void Fill(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public abstract TEntity GetInternal(int id, IDbConnection connection, IDbTransaction transaction);

        protected void CallInTransaction(IsolationLevel isolationLevel, Action<IDbConnection, IDbTransaction> action)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(isolationLevel))
                {
                    action(connection, transaction);

                    connection.Close();
                }
            }
        }

        protected T CallInTransaction<T>(IsolationLevel isolationLevel, Func<IDbConnection, IDbTransaction, T> action)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(isolationLevel))
                {
                    var result = action(connection, transaction);
                    connection.Close();

                    return result;
                }
            }
        }
    }
}