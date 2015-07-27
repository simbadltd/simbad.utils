using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DapperExtensions;

namespace Simbad.Utils.Orm
{
    public abstract class RepositoryBase<TEntity, TId, TContext> : IRepository<TEntity, TId, TContext> where TEntity : class, IEntity<TId>, IAggregationRoot
    {
        protected int? CommandTimeout { get; private set; }

        protected IsolationLevel IsolationLevel { get; private set; }

        protected IConnectionFactory ConnectionFactory { get; private set; }

        protected RepositoryBase(IConnectionFactory connectionFactory, IsolationLevel isolationLevel, int? commandTimeout = null)
        {
            CommandTimeout = commandTimeout;
            IsolationLevel = isolationLevel;
            ConnectionFactory = connectionFactory;
        }

        public TEntity FindSingle(ISpecification<TEntity, TId> specification = null)
        {
            return
                CallInTransactionAndReturn(
                    (connection, transaction) =>
                        FindSingle(connection, transaction, GroupOperator.And, GetDefaultContext(), specification));
        }

        public TEntity FindSingle(TContext context, ISpecification<TEntity, TId> specification = null)
        {
            return
                CallInTransactionAndReturn(
                    (connection, transaction) =>
                        FindSingle(connection, transaction, GroupOperator.And, context, specification));
        }

        public TEntity FindSingle(IDbConnection connection, IDbTransaction transaction, ISpecification<TEntity, TId> specification = null)
        {
            return FindSingle(connection, transaction, GroupOperator.And, GetDefaultContext(), specification);
        }

        public TEntity FindSingle(GroupOperator op, TContext context, params ISpecification<TEntity, TId>[] specifications)
        {
            return CallInTransactionAndReturn((connection, transaction) => FindSingle(connection, transaction, op, context, specifications));
        }

        public TEntity FindSingle(GroupOperator op, params ISpecification<TEntity, TId>[] specifications)
        {
            return CallInTransactionAndReturn((connection, transaction) => FindSingle(connection, transaction, op, GetDefaultContext(), specifications));
        }

        public TEntity FindSingle(IDbConnection connection, IDbTransaction transaction, GroupOperator op,
            params ISpecification<TEntity, TId>[] specifications)
        {
            return FindSingle(connection, transaction, op, GetDefaultContext(), specifications);
        }

        public TEntity FindSingle(IDbConnection connection, IDbTransaction transaction, TContext context,
            ISpecification<TEntity, TId> specification = null)
        {
            return FindSingle(connection, transaction, GroupOperator.And, context, specification);
        }

        public TEntity FindSingle(IDbConnection connection, IDbTransaction transaction, GroupOperator op, TContext context,
            params ISpecification<TEntity, TId>[] specifications)
        {
            return FindMany(connection, transaction, op, context, specifications).SingleOrDefault();
        }

        public ICollection<TEntity> FindMany(ISpecification<TEntity, TId> specification = null)
        {
            return CallInTransactionAndReturn((connection, transaction) => FindMany(connection, transaction, GroupOperator.And, GetDefaultContext(), specification));
        }

        public ICollection<TEntity> FindMany(TContext context, ISpecification<TEntity, TId> specification = null)
        {
            return CallInTransactionAndReturn((connection, transaction) => FindMany(connection, transaction, GroupOperator.And, context, specification));
        }

        public ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction, TContext context,
            ISpecification<TEntity, TId> specification = null)
        {
            return FindMany(connection, transaction, GroupOperator.And, context, specification);
        }

        public ICollection<TEntity> FindMany(GroupOperator op, params ISpecification<TEntity, TId>[] specifications)
        {
            return CallInTransactionAndReturn((connection, transaction) => FindMany(connection, transaction, op, GetDefaultContext(), specifications));
        }

        public ICollection<TEntity> FindMany(GroupOperator op, TContext context, params ISpecification<TEntity, TId>[] specifications)
        {
            return CallInTransactionAndReturn((connection, transaction) => FindMany(connection, transaction, op, context, specifications));
        }

        public ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction, ISpecification<TEntity, TId> specification = null)
        {
            return FindMany(connection, transaction, GroupOperator.And, GetDefaultContext(), specification);
        }

        public ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction, GroupOperator op,
            params ISpecification<TEntity, TId>[] specifications)
        {
            return FindMany(connection, transaction, op, GetDefaultContext(), specifications);
        }

        public virtual ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction, GroupOperator op, TContext context,
            params ISpecification<TEntity, TId>[] specifications)
        {
            var predicate = MergeSpecifications(specifications, op);
            return connection.GetList<TEntity>(predicate: predicate, transaction: transaction,
                commandTimeout: CommandTimeout).ToList();
        }

        public virtual TId Save(IDbConnection connection, IDbTransaction transaction, TEntity entity)
        {
            return SaveInternal<TEntity, TId>(connection, transaction, entity);
        }

        protected T2 SaveInternal<T1, T2>(IDbConnection connection, IDbTransaction transaction, T1 entity, T1 originalEntity = null) where T1 : class, IEntity<T2>
        {
            if (entity.State == EntityState.New)
            {
                var id = (T2)connection.Insert(entity: entity, transaction: transaction, commandTimeout: CommandTimeout);
                entity.Id = id;
                return id;
            }

            if (entity.State == EntityState.Updated)
            {
                if (originalEntity == null)
                {
                    originalEntity = connection.Get<T1>(id: entity.Id, transaction: transaction,
                        commandTimeout: CommandTimeout);
                }

                var isChanged = IsChanged<T1, T2>(entity, originalEntity);
                if (isChanged)
                {
                    connection.Update(entity, transaction, CommandTimeout);
                }

                return entity.Id;
            }

            throw new InvalidOperationException(string.Format("Cannot save entity with state:[{0}]", entity.State));
        }

        protected bool IsChanged<TEntity2, TId2>(TEntity2 entityToSave, TEntity2 originalEntity) where TEntity2 : class, IEntity<TId2>
        {
            var map = DapperExtensions.DapperExtensions.GetMap<TEntity2>();
            var properties = map.Properties.Select(p => p.PropertyInfo).ToList();

            return CompareHelper.CompareObjects(entityToSave, originalEntity, properties) == false;
        }

        public virtual void Delete(IDbConnection connection, IDbTransaction transaction, TEntity entity)
        {
            if (entity.State == EntityState.Deleted)
            {
                connection.Delete(entity: entity, transaction: transaction, commandTimeout: CommandTimeout);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Cannot delete entity with state:[{0}]", entity.State));
            }
        }

        private static IPredicate MergeSpecifications(ICollection<ISpecification<TEntity, TId>> specifications, GroupOperator op)
        {
            if (specifications == null || specifications.Any() == false)
            {
                return null;
            }

            var notNullSpecifications = specifications.Where(x => x != null).ToList();
            if (notNullSpecifications.Any() == false)
            {
                return null;
            }

            if (notNullSpecifications.Count == 1)
            {
                return notNullSpecifications.First().Execute();
            }

            return Predicates.Group(op, notNullSpecifications.Select(p => p.Execute()).ToArray());
        }

        protected void CallInTransaction(Action<IDbConnection, IDbTransaction> action)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(IsolationLevel))
                {
                    action(connection, transaction);

                    connection.Close();
                }
            }
        }

        protected T CallInTransactionAndReturn<T>(Func<IDbConnection, IDbTransaction, T> action)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction(IsolationLevel))
                {
                    var result = action(connection, transaction);
                    connection.Close();

                    return result;
                }
            }
        }

        protected virtual TContext GetDefaultContext()
        {
            return default(TContext);
        }
    }
}