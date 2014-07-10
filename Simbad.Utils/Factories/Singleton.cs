namespace Simbad.Utils.Factories
{
    public class Singleton<T> where T : class, new()
    {
        private static volatile T _instance;

        private readonly static object _syncRoot = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}