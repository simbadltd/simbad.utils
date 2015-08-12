using System.Collections.Generic;
using System.Data;
using DapperExtensions;

namespace Simbad.Utils.Orm
{
    public interface IRepository<TEntity, TId, in TContext>
        where TEntity : class, IEntity<TId>, IAggregationRoot
        where TId : struct
    {
        TEntity FindSingle(GroupOperator op, TContext context, params ISpecification<TEntity, TId>[] specifications);

        TEntity FindSingle(GroupOperator op, params ISpecification<TEntity, TId>[] specifications);

        TEntity FindSingle(IDbConnection connection, IDbTransaction transaction, GroupOperator op,
                           params ISpecification<TEntity, TId>[] specifications);

        TEntity FindSingle(IDbConnection connection, IDbTransaction transaction, GroupOperator op,
                           TContext context, params ISpecification<TEntity, TId>[] specifications);

        TEntity FindSingle(ISpecification<TEntity, TId> specification = null);

        TEntity FindSingle(TContext context, ISpecification<TEntity, TId> specification = null);

        TEntity FindSingle(IDbConnection connection, IDbTransaction transaction,
                           ISpecification<TEntity, TId> specification = null);

        TEntity FindSingle(IDbConnection connection, IDbTransaction transaction, TContext context,
                           ISpecification<TEntity, TId> specification = null);

        ICollection<TEntity> FindMany(ISpecification<TEntity, TId> specification = null);

        ICollection<TEntity> FindMany(TContext context, ISpecification<TEntity, TId> specification = null);

        ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction,
                                      ISpecification<TEntity, TId> specification = null);

        ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction, TContext context,
                                      ISpecification<TEntity, TId> specification = null);

        ICollection<TEntity> FindMany(GroupOperator op, params ISpecification<TEntity, TId>[] specifications);

        ICollection<TEntity> FindMany(GroupOperator op, TContext context,
                                      params ISpecification<TEntity, TId>[] specifications);

        ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction, GroupOperator op,
                                      params ISpecification<TEntity, TId>[] specifications);

        ICollection<TEntity> FindMany(IDbConnection connection, IDbTransaction transaction, GroupOperator op,
                                      TContext context, params ISpecification<TEntity, TId>[] specifications);

        TId Save(IDbConnection connection, IDbTransaction transaction, TEntity entity);

        void Delete(IDbConnection connection, IDbTransaction transaction, TEntity entity);
    }
}