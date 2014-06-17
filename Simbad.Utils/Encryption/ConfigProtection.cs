using System;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using System.Web.Hosting;

namespace Simbad.Utils.Encryption
{
    public class ConfigProtection
    {
        public static void EncryptAppConfigSection(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                return;
            }

            var appPath = Path.Combine(
                AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                AppDomain.CurrentDomain.SetupInformation.ApplicationName);

            var configuration = ConfigurationManager.OpenExeConfiguration(appPath);

            EncryptConfigSectionInternal(configuration, appPath, sectionName);
        }

        public static void EncryptWebConfigSection(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                return;
            }

            var appPath = HostingEnvironment.ApplicationVirtualPath;
            var configuration = WebConfigurationManager.OpenWebConfiguration(appPath);

            EncryptConfigSectionInternal(configuration, appPath, sectionName);
        }

        private static void EncryptConfigSectionInternal(System.Configuration.Configuration configuration, string appPath, string sectionName)
        {
            if (configuration == null)
            {
                throw new FileNotFoundException("Could not find config file in " + appPath);
            }

            var section = configuration.GetSection(sectionName);

            if (section == null)
            {
                throw new InvalidOperationException("Could not find section " + sectionName + ". It should be present.");
            }

            if (section.ElementInformation.IsLocked)
            {
                throw new InvalidOperationException("Could not encrypt section " + section + ". It is locked.");
            }

            if (section.SectionInformation.IsProtected)
            {
                return;
            }

            section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
            section.SectionInformation.ForceSave = true;
            configuration.Save(ConfigurationSaveMode.Full);
        }
    }
}