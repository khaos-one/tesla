﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tesla.Cryptography.Tests
{
    [TestClass]
    public class RC4ManagedTests
    {
        [TestMethod]
        public void EncryptDecryptTest()
        {
            var sampleData = new byte[]
            {
                0x32, 0x31, 0x30, 0x39, 0x38, 0x37, 0x36, 0x35, 0x34, 0x33, 0x32, 0x31, 0x30, 0x39, 0x38, 0x37, 0x36, 0x35,
                0x34, 0x33, 0x32, 0x31, 0x30, 0x39, 0x38, 0x37, 0x36, 0x35, 0x34, 0x33, 0x32, 0x31, 0x30, 0x39, 0x38,
                0x37, 0x36, 0x35, 0x34, 0x33, 0x32, 0x31, 0x30, 0x39, 0x38, 0x37, 0x36, 0x35, 0x34, 0x33, 0x32, 0x31,
                0x30, 0x39, 0x38, 0x37, 0x36, 0x35, 0x34, 0x33, 0x32, 0x31, 0x30
            };
            var key = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a};
            var originalHash = new byte[]
            {
                0xdf, 0xd2, 0x80, 0x7f, 0x7b, 0xd2, 0xb0, 0xf9, 0xa4, 0x4e, 0xf0, 0x29, 0x61, 0x49, 0xa1, 0x35, 0x35,
                0x64, 0x5f, 0x94, 0xbd, 0x70, 0x0b, 0xd2, 0x1a, 0x0d, 0x93, 0xe1, 0xe6, 0xec, 0x55, 0x20, 0x85, 0xc1,
                0xb4, 0x6e, 0xe9, 0x3a, 0x68, 0xa3, 0x16, 0x91, 0x20, 0x88, 0xab, 0x2b, 0x0f, 0xa6, 0x0c, 0xb8, 0xc4,
                0x6d, 0x3f, 0x57, 0xd7, 0x23, 0x36, 0xe6, 0x32, 0x3f, 0xbe, 0xca, 0x88
            };

            using (var cipher = new RC4Managed())
            using (var transform = cipher.CreateEncryptor(key, null))
            {
                var encrypted = transform.TransformFinalBlock(sampleData, 0, sampleData.Length);
                var decrypted = transform.TransformFinalBlock(encrypted, 0, encrypted.Length);

                CollectionAssert.AreEqual(sampleData, decrypted);
            }
        }
    }
}
