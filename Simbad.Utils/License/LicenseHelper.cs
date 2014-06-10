using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management;

namespace Simbad.Utils.License
{
    public class LicenseHelper
    {
        public static string GetMacAddress()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration where IPEnabled=true");
            var objects = searcher.Get().Cast<ManagementObject>();
            var mac = (from o in objects orderby o["IPConnectionMetric"] select o["MACAddress"].ToString()).FirstOrDefault();
            return mac;
        }

        public static string GetDriveKey(int id)
        {
            var signature = GetDeviceArray("Win32_DiskDrive", "Signature");
            if (signature == null || signature.Count == 0)
            {
                return string.Empty;
            }

            if (id > signature.Count - 1)
            {
                id = 0;
            }

            return signature[id];
        }

        private static List<string> GetDeviceArray(string fromWin32Class, string classItemAdd)
        {
            var result = new List<string>();
            var searcher = new ManagementObjectSearcher("SELECT * FROM " + fromWin32Class);

            try
            {
                result.AddRange(from ManagementObject obj in searcher.Get() select obj[classItemAdd].ToString().Trim());
            }
            catch (Exception)
            {
            }

            return result;
        }

        public static void ProtectSectionIfNeeded(string exePath, string sectionName, string protectionProvider = "RSAProtectedConfigurationProvider")
        {
            var config = ConfigurationManager.OpenExeConfiguration(exePath);
            var section = config.GetSection(sectionName);

            if (section != null && !section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection(protectionProvider);
            }
        }
    }
}
