using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;

namespace Simbad.Utils.Orm
{
    public abstract class RepositoryBase<TEntity, TId, TContext> : IRepository<TEntity, TId, TContext>
        where TEntity : class, IEntity<TId>, IAggregationRoot
        where TId : struct
    {
        protected int? CommandTimeout { get; private set; }

        protected IsolationLevel IsolationLevel { get; private set; }

        protected IConnectionFactory ConnectionFactory { get; private set; }

        protected RepositoryBase(IConnectionFactory connectionFactory, IsolationLevel isolationLevel,
                                 int? commandTimeout = null)
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

        public TEntity FindSingle(IDbConnection connection, IDbTransaction transaction,
                                  ISpecification<TEntity, TId> specification = null)
        {
            return FindSingle(connection, transaction, GroupOperator.And, GetDefaultContext(), specification);
        }

        public TEntity FindSingle(GroupOperator op, TContext context,
                                  params ISpecification<TEntity, TId>[] specifications)
        {
            return
                CallInTransactionAndReturn(
                    (connection, transaction) => FindSingle(connection, transaction, op, context, specifications));
        }

        public TEntity FindSingle(GroupOperator op, params ISpecification<TEntity, TId>[] specifications)
        {
            return
                CallInTransactionAndReturn(
                    (connection, transaction) =>
                    FindSingle(connection, transaction, op, GetDefaultContext(), specifications));
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

        public TEntity FindSingle(IDbConnection connection, IDbTransaction transaction, GroupOperator op,
                                  TContext context,
                                  params ISpecification<TEntity, TId>[] specifications)
        {
            return FindMany(connection, transaction, op, context, specifications).FirstOrDefault();
        }

        public ICollection<TEntity> FindMany(ISpecification<TEntity, TId> specification = null)
        {
            return
                CallInTransactionAndReturn(
                    (connection, transaction) =>
                    FindMany(connection, transaction, GroupOperator.And, GetDefaultContext(), specification));
        }

        public ICollection<TEntity> FindMany(TContext context, ISpecification<TEntity, TId> specification = null)
        {
            return
                CallInTransactionAndReturn(
                    (connection, transaction) =>
                    FindMany(connection, transaction, GroupOperator.And, context, specification));
        }

        public ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction, TContext context,
                                             ISpecification<TEntity, TId> specification = null)
        {
            return FindMany(connection, transaction, GroupOperator.And, context, specification);
        }

        public ICollection<TEntity> FindMany(GroupOperator op, params ISpecification<TEntity, TId>[] specifications)
        {
            return
                CallInTransactionAndReturn(
                    (connection, transaction) =>
                    FindMany(connection, transaction, op, GetDefaultContext(), specifications));
        }

        public ICollection<TEntity> FindMany(GroupOperator op, TContext context,
                                             params ISpecification<TEntity, TId>[] specifications)
        {
            return
                CallInTransactionAndReturn(
                    (connection, transaction) => FindMany(connection, transaction, op, context, specifications));
        }

        public ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction,
                                             ISpecification<TEntity, TId> specification = null)
        {
            return FindMany(connection, transaction, GroupOperator.And, GetDefaultContext(), specification);
        }

        public ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction, GroupOperator op,
                                             params ISpecification<TEntity, TId>[] specifications)
        {
            return FindMany(connection, transaction, op, GetDefaultContext(), specifications);
        }

        public virtual ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction,
                                                     GroupOperator op, TContext context,
                                                     params ISpecification<TEntity, TId>[] specifications)
        {
            var predicate = MergeSpecifications<TEntity>(specifications, op);
            return connection.GetList<TEntity>(predicate: predicate, transaction: transaction,
                                               commandTimeout: CommandTimeout).ToList();
        }

        public virtual TId Save(IDbConnection connection, IDbTransaction transaction, TEntity entity)
        {
            return SaveInternal<TEntity, TId>(connection, transaction, entity);
        }

        protected T2 SaveInternal<T1, T2>(IDbConnection connection, IDbTransaction transaction, T1 entity,
                                          T1 originalEntity = null) where T1 : class, IEntity<T2>
        {
            if (entity.State == EntityState.New)
            {
                var id =
                    (T2) connection.Insert(entity: entity, transaction: transaction, commandTimeout: CommandTimeout);
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

        protected bool IsChanged<TEntity2, TId2>(TEntity2 entityToSave, TEntity2 originalEntity)
            where TEntity2 : class, IEntity<TId2>
        {
            var map = DapperExtensions.DapperExtensions.GetMap<TEntity2>();
            var properties = map.Properties.Select(p => p.PropertyInfo).ToList();

            return CompareHelper.CompareObjects(entityToSave, originalEntity, properties) == false;
        }

        protected void RawDelete<T>(IDbConnection connection, IDbTransaction transaction,
                              params ISpecification<T, TId>[] specifications) where T : class, IEntity<TId>
        {
            RawDelete(connection, transaction, GroupOperator.And, specifications);
        }

        protected void RawDelete<T>(IDbConnection connection, IDbTransaction transaction, GroupOperator op,
                                    params ISpecification<T, TId>[] specifications) where T : class, IEntity<TId>
        {
            var predicate = MergeSpecifications(specifications, op);
            connection.Delete<TEntity>(predicate, transaction, CommandTimeout);
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

        private static IPredicate MergeSpecifications<T>(ICollection<ISpecification<T, TId>> specifications,
                                                      GroupOperator op) where T : class, IEntity<TId>
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

        public static ICollection<TOne> QueryWithOneToManyRelations<TOne, TMany>(IDbConnection cnn, IClassMapper<TOne> oneMapper,
                                                           IClassMapper<TMany> manyMapper,
                                                           Func<TOne, ICollection<TMany>> property,
                                                           string manyForeignKeyColumnName,
                                                           string oneEntityColumnFilter = null,
                                                           string oneEntityIdFilter = null, dynamic param = null,
                                                           IDbTransaction transaction = null,
                                                           bool buffered = true, int? commandTimeout = null,
                                                           CommandType? commandType = null)
            where TOne : class, IEntity<TId>
            where TMany : class, IEntity<TId>
        {
            var onePrimaryKeyPropertyName = oneMapper.Properties.Single(x => x.KeyType == KeyType.Identity).Name;
            var manyPrimaryKeyPropertyName = manyMapper.Properties.Single(x => x.KeyType == KeyType.Identity).Name;
            var splitOn = string.Concat(onePrimaryKeyPropertyName, ",", manyPrimaryKeyPropertyName);
            var sql = BuildJoinSql(oneMapper, manyMapper, "LEFT", manyForeignKeyColumnName, oneEntityColumnFilter,
                                   oneEntityIdFilter);
            var cache = new Dictionary<TId, TOne>();
            cnn.Query<TOne, TMany, TOne>(sql, (one, many) =>
                {
                    if (!cache.ContainsKey(one.Id))
                    {
                        cache.Add(one.Id, one);
                    }

                    var localOne = cache[one.Id];

                    if (many != null)
                    {
                        var list = property(localOne);
                        list.Add(many);
                    }

                    return localOne;
                },
                                         param as object, transaction, buffered, splitOn, commandTimeout, commandType);

            return cache.Values;
        }

        private static string BuildJoinSql(IClassMapper oneMapper, IClassMapper manyMapper, string joinType,
                                           string manyForeignKeyColumnName, string oneEntityColumnFilter = null,
                                           string oneEntityIdFilter = null)
        {
            var oneProperties = GetMappedColumnsSqlStr("one", oneMapper);
            var manyProperties = GetMappedColumnsSqlStr("many", manyMapper);
            var oneTableName = oneMapper.TableName;
            var manyTableName = manyMapper.TableName;
            var onePrimaryKeyColumnName = oneMapper.Properties.Single(x => x.KeyType == KeyType.Identity).ColumnName;

            var sb = new StringBuilder();
            sb.AppendFormat(
                "SELECT {0}, {1} FROM {2} one {6} JOIN {3} many ON one.{4} = many.{5}",
                oneProperties, manyProperties, oneTableName, manyTableName, onePrimaryKeyColumnName,
                manyForeignKeyColumnName, joinType);

            if (string.IsNullOrEmpty(oneEntityIdFilter))
            {
                sb.AppendFormat(" WHERE one.{0} = {1}", oneEntityColumnFilter, oneEntityIdFilter);
            }

            return sb.ToString();
        }

        private static string GetMappedColumnsSqlStr(string prefix, IClassMapper mapper)
        {
            var sb = new StringBuilder();
            var i = 0;

            foreach (var propertyMap in mapper.Properties)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.AppendFormat("{2}.{0} as {1}", propertyMap.ColumnName, propertyMap.Name, prefix);
                i++;
            }

            return sb.ToString();
        }

        protected void SaveChilds<TCollectionEntity>(IDbConnection connection, IDbTransaction transaction,
                                                     TEntity entity,
                                                     Expression<Func<TEntity, ICollection<TCollectionEntity>>>
                                                         newCollectionProperty,
                                                     Expression<Func<TCollectionEntity, TId>> foreignKeyProperty,
                                                     Func
                                                         <IDbConnection, IDbTransaction, TEntity,
                                                         ICollection<TCollectionEntity>> oldCollectionFunc
            )
            where TCollectionEntity : class, IEntity<TId>
        {
            var originalChildEntities = oldCollectionFunc(connection, transaction, entity);
            var newChildEntities =
                ((PropertyInfo) ((MemberExpression) newCollectionProperty.Body).Member).GetValue(entity, null) as
                ICollection<TCollectionEntity>;

            if (newChildEntities != null && newChildEntities.Any())
            {
                foreach (var newChildEntity in newChildEntities)
                {
                    var originalEntity = originalChildEntities.FirstOrDefault(x => x.Id.Equals(newChildEntity.Id));
                    var isNew = originalEntity == null;
                    newChildEntity.State = isNew ? EntityState.New : EntityState.Updated;

                    // set foreign key
                    ((PropertyInfo) ((MemberExpression) foreignKeyProperty.Body).Member).SetValue(newChildEntity,
                                                                                                  entity.Id, null);

                    var id = SaveInternal<TCollectionEntity, TId>(connection, transaction, newChildEntity,
                                                                  originalEntity);
                    newChildEntity.Id = id;
                }
            }

            if (originalChildEntities != null && originalChildEntities.Any())
            {
                foreach (var originalChildEntity in originalChildEntities)
                {
                    var childEntityWasNotDeleted = newChildEntities != null &&
                                                   newChildEntities.Any(x => x.Id.Equals(originalChildEntity.Id));

                    if (childEntityWasNotDeleted)
                    {
                        continue;
                    }

                    connection.Delete(originalChildEntity, transaction, CommandTimeout);
                }
            }
        }
    }
}