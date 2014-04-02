namespace Simbad.Utils.Factories
{
    public interface IMultiton<T1,T2>
    {
        T1 Get(T2 key);
    }
}
