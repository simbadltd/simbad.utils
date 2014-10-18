using System;
using System.IO;

using Newtonsoft.Json;

using Simbad.Utils.Utils;

namespace Simbad.Utils.Configuration
{
    public class ConfigurationManager<T>
        where T : class, new()
    {
        public const string DEFAULT_CONFIGURATION_FILE = "~\\settings.conf";

        private static readonly Lazy<ConfigurationManager<T>> _current = new Lazy<ConfigurationManager<T>>(CurrentFactory, true);

        private Lazy<T> _settings;

        public ConfigurationManager()
        {
            _settings = new Lazy<T>(SettingsFactory, true);
        }

        public static ConfigurationManager<T> Current
        {
            get
            {
                return _current.Value;
            }
        }

        public T Settings
        {
            get
            {
                return _settings.Value;
            }
        }

        private static ConfigurationManager<T> CurrentFactory()
        {
            return new ConfigurationManager<T>();
        }

        private T SettingsFactory()
        {
            return Load();
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
            _settings = new Lazy<T>(SettingsFactory);
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
