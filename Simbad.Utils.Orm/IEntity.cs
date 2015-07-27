namespace Simbad.Utils.Orm
{
    public interface IEntity<T>
    {
        T Id { get; set; }

        EntityState State { get; set; }
    }
}