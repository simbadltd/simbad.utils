using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simbad.Utils.Encryption;

namespace Simbad.Utils.Tests
{
    [TestClass]
    public class AesEncryptionTests
    {
        private const string Key = "F07E-251D-B99D-CF7F-922C-2868-6AC3-51F4-1741-9BFA";

        private const string Text =
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.";

        [TestMethod]
        public void ShouldEncryptAndDecryptCorrectly()
        {
            //Given
            var encryptedText = AesEncryption.Encrypt(Text, Key);

            //When
            var decryptedText = AesEncryption.Decrypt(encryptedText, Key);

            //Then
            Assert.AreEqual(decryptedText, Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowExceptionIfInvalidKeyPassedWhenEncrypt()
        {
            AesEncryption.Encrypt(Text, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ShouldThrowExceptionIfNotBase64DataPassedWhenDecrypt()
        {
            AesEncryption.Decrypt(Text, null);
        }
    }
}