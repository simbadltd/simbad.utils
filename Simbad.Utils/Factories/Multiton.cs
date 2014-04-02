using System.Collections.Concurrent;

namespace Simbad.Utils.Factories
{
    public abstract class Multiton<T> : Multiton<T, string>
    {
    }

    public abstract class Multiton<T1, T2> : IMultiton<T1,T2>
    {
        private readonly ConcurrentDictionary<T2, T1> _cache = new ConcurrentDictionary<T2, T1>();

        public T1 Get(T2 key)
        {
            return _cache.GetOrAdd(key, CreateByKey);
        }

        protected abstract T1 CreateByKey(T2 key);
    }
}
