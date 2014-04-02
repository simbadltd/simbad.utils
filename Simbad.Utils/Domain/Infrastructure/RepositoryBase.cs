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

        public void Persist(TEntity item)
        {
            if (item.IsNew)
            {
                Add(item);
            }
            else
            {
                Update(item);
            }
        }

        public void Add(TEntity item)
        {
            using (var cn = ConnectionFactory.CreateConnection())
            {
                cn.Open();

                using (var transaction = cn.BeginTransaction())
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

        public void Update(TEntity item)
        {
            using (var cn = ConnectionFactory.CreateConnection())
            {
                cn.Open();

                using (var transaction = cn.BeginTransaction())
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

        public void Remove(TEntity item)
        {
            using (var cn = ConnectionFactory.CreateConnection())
            {
                cn.Open();

                using (var transaction = cn.BeginTransaction())
                {
                    RemoveInternal(item, cn, transaction);
                    transaction.Commit();
                }
            }
        }

        protected virtual void RemoveInternal(TEntity item, IDbConnection connection, IDbTransaction transaction)
        {
        }

        public virtual TEntity Get(int id)
        {
            using (var cn = ConnectionFactory.CreateConnection())
            {
                cn.Open();
                return GetInternal(id, cn, null);
            }
        }

        protected virtual TEntity GetInternal(int id, IDbConnection connection, IDbTransaction transaction)
        {
            return null;
        }
    }
}