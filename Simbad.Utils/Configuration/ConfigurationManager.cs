using System;
using System.IO;

using Newtonsoft.Json;

using Simbad.Utils.Factories;
using Simbad.Utils.Utils;

namespace Simbad.Utils.Configuration
{
    public class ConfigurationManager<T> : Singleton<ConfigurationManager<T>>
        where T : class, new()
    {
        public const string DEFAULT_CONFIGURATION_FILE = "~\\settings.conf";

        private static readonly object _syncRoot = new object();

        private volatile T _settings;

        public T Settings
        {
            get
            {
                if (_settings == null)
                {
                    lock (_syncRoot)
                    {
                        if (_settings == null)
                        {
                            _settings = Load();
                        }
                    }
                }

                return _settings;
            }
        }

        public T Load(string configurationFile = DEFAULT_CONFIGURATION_FILE)
        {
            T result = null;
            var settingsFile = PathUtils.ToAbsolutePath(configurationFile);

            if (File.Exists(settingsFile))
            {
                result = TryDeserialize(File.ReadAllText(settingsFile));
            }

            if (result == null)
            {
                result = new T();
            }

            return result;
        }

        public void Save(string json, string configurationFile = DEFAULT_CONFIGURATION_FILE)
        {
            var settings = TryDeserialize(json);
            Save(settings, configurationFile);
        }

        public void Save(T settings, string configurationFile = DEFAULT_CONFIGURATION_FILE)
        {
            File.WriteAllText(PathUtils.ToAbsolutePath(configurationFile), TrySerialize(settings));

            lock (_syncRoot)
            {
                _settings = null;
            }
        }

        private static T TryDeserialize(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return new T();
            }
        }

        private static string TrySerialize(T settings)
        {
            try
            {
                return JsonConvert.SerializeObject(settings);
            }
            catch
            {
                return "{}";
            }
        }
    }
}
