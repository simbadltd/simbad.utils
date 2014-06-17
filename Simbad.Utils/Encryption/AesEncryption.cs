using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Simbad.Utils.Encryption
{
    public static class AesEncryption
    {
        private const string Salt = "560A10CD-6346-4CFA-T2E8-671F9B6T9UA9";

        public static string Encrypt(string text, string key)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            using (var aesAlg = CreateAes(key))
            {
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(text);
                        }
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static bool TryDecrypt(string cipherText, string key, out string decryptedString)
        {
            try
            {
                decryptedString = Decrypt(cipherText, key);

                return true;
            }
            catch (Exception)
            {
                decryptedString = null;

                return false;
            }
        }

        public static string Decrypt(string cipherText, string key)
        {
            Guard.NotNull(cipherText, "cipherText");

            if (!IsBase64String(cipherText))
            {
                throw new InvalidDataException("The cipherText input parameter is not base64 encoded");
            }

            string text;

            using (var aesAlg = CreateAes(key))
            {
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                var cipher = Convert.FromBase64String(cipherText);

                using (var ms = new MemoryStream(cipher))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            text = sr.ReadToEnd();
                        }
                    }
                }
            }

            return text;
        }

        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();

            return (base64String.Length % 4 == 0) &&
                   Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        private static Aes CreateAes(string key)
        {
            Guard.NotNull(key, "key");

            var aesAlg = Aes.Create();
            Guard.NotNull(aesAlg, "aesAlg");

            var saltBytes = Encoding.ASCII.GetBytes(Salt);
            var saltedKey = new Rfc2898DeriveBytes(key, saltBytes);
            aesAlg.Key = saltedKey.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = saltedKey.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
    }
}