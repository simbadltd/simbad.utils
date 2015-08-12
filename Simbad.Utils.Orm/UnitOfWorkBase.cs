using System.Collections;

namespace Simbad.Utils.Orm
{
    public abstract class UnitOfWorkBase
    {
        protected readonly object Sync = new object();

        protected IList New { get; private set; }

        protected IList Updated { get; private set; }

        protected IList Deleted { get; private set; }

        protected UnitOfWorkBase()
        {
            New = new ArrayList();
            Updated = new ArrayList();
            Deleted = new ArrayList();
        }

        public void Create<TEntity, TId>(TEntity entity) where TEntity : IEntity<TId>, IAggregationRoot
        {
            lock (Sync)
            {
                entity.State = EntityState.New;
                New.Add(entity);
            }
        }

        public void Update<TEntity, TId>(TEntity entity) where TEntity : IEntity<TId>, IAggregationRoot
        {
            lock (Sync)
            {
                entity.State = EntityState.Updated;
                Updated.Add(entity);
            }
        }

        public void Delete<TEntity, TId>(TEntity entity) where TEntity : IEntity<TId>, IAggregationRoot
        {
            lock (Sync)
            {
                entity.State = EntityState.Deleted;
                Deleted.Add(entity);
            }
        }

        public abstract void Commit();
    }
}