namespace Simbad.Utils.Domain.Infrastructure
{
    /// <summary>
    /// The repository interface.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The domain entity
    /// </typeparam>
    public interface IRepository<TEntity> where TEntity: EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        void Add(TEntity item);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        void Remove(TEntity item);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        void Update(TEntity item);

        /// <summary>
        /// Gets item.
        /// </summary>
        /// <param name="id">
        /// Item identificator
        /// </param>
        TEntity Get(int id);
    }
}