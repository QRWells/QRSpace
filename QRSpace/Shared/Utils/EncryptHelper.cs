using System;
using System.Security.Cryptography;

namespace QRSpace.Shared.Utils
{
    public static class EncryptHelper
    {
        private const string PublicKey = "m0F6s9A9X1EL&%3u";// A 32-bit key

        private const string Iv = "o#gV1kiWzRMR^kz%";//A 32-bit key

        // ReSharper disable once InconsistentNaming
        public static string EncryptWithAES(string origin)
        {
            var keyArray = System.Text.Encoding.UTF8.GetBytes(PublicKey);
            var toEncryptArray = System.Text.Encoding.UTF8.GetBytes(origin);
            var rijndael = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7,
                IV = System.Text.Encoding.UTF8.GetBytes(Iv)
            };
            var cTransform = rijndael.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        // ReSharper disable once InconsistentNaming
        public static string DecryptWithAES(string password)
        {
            var keyArray = System.Text.Encoding.UTF8.GetBytes(PublicKey);
            var toEncryptArray = Convert.FromBase64String(password);
            var rijndael = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7,
                IV = System.Text.Encoding.UTF8.GetBytes(Iv)
            };
            var cTransform = rijndael.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return System.Text.Encoding.UTF8.GetString(resultArray);
        }
    }
}