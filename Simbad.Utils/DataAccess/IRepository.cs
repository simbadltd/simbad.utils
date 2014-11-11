﻿using System.Data;

using Simbad.Utils.Domain;

namespace Simbad.Utils.DataAccess
{
    /// <summary>
    /// The repository interface.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The domain entity
    /// </typeparam>
    public interface IRepository<TEntity> where TEntity : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        int Add(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        void Remove(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        void Update(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Gets item.
        /// </summary>
        /// <param name="id">
        /// Item identificator
        /// </param>
        TEntity Get(int id, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        TEntity GetInternal(int id, IDbConnection connection, IDbTransaction transaction);

        /// <summary>
        /// Add or update item. Depend on IsNew flag.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        int Persist(TEntity item, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        int AddInternal(TEntity item, IDbConnection connection, IDbTransaction transaction);

        void UpdateInternal(TEntity item, IDbConnection connection, IDbTransaction transaction);

        void RemoveInternal(TEntity item, IDbConnection connection, IDbTransaction transaction);
    }
}