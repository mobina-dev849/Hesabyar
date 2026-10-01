using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common.Security
{
    public static class SecurityHelper
    {


        //SHA-256
        public static string ComputeSha256Hash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            using SHA256 sha256 = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(text);

            byte[] hash = sha256.ComputeHash(bytes);

            return Convert.ToHexString(hash).ToLower();
        }



        //AESتبدیل
        public static string Encrypt(string plainText, string key)
        {
            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.GenerateIV();

            using MemoryStream ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (CryptoStream cs = new CryptoStream(
                ms,
                aes.CreateEncryptor(),
                CryptoStreamMode.Write))
            {
                using StreamWriter sw = new StreamWriter(cs);

                sw.Write(plainText);
            }


            return Convert.ToBase64String(ms.ToArray());
        }



        //AESبازگشت
        public static string Decrypt(string encryptedText, string key)
        {
            byte[] encryptedData = Convert.FromBase64String(encryptedText);

            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(key);

            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(encryptedData, 0, iv, 0, iv.Length);

            aes.IV = iv;

            using MemoryStream ms = new MemoryStream();

            using (CryptoStream cs = new CryptoStream(
                ms,
                aes.CreateDecryptor(),
                CryptoStreamMode.Write))
            {
                cs.Write(
                    encryptedData,
                    iv.Length,
                    encryptedData.Length - iv.Length);

                cs.FlushFinalBlock();
            }

            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
