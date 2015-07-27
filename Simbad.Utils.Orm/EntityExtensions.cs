namespace Simbad.Utils.Orm
{
    public static class EntityExtensions
    {
        public static bool Exists<T>(this IEntity<T> entity)
        {
            return entity != null;
        }
    }
}