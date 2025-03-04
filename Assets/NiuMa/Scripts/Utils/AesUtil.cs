using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using XLua;

namespace NiuMa
{
    [LuaCallCSharp]
    public static class AesUtil
    {
        private static byte[] key = Encoding.UTF8.GetBytes("Lc1Zi8481lfH0F72"); // 16字节的密钥
        private static byte[] iv = Encoding.UTF8.GetBytes("OkxNe6vsRD1d3vUe");  // 16字节的初始化向量
        private static byte[] key1 = Encoding.UTF8.GetBytes("73qaS4K1PcW4n9uN"); // 16字节的密钥
        private static byte[] iv1 = Encoding.UTF8.GetBytes("XcTyAJe8o1OGVlMo");  // 16字节的初始化向量

        public static string Encrypt(string plainText)
        {
            return EncryptImpl(plainText, key, iv);
        }

        public static string Decrypt(string cipherText)
        {
            return DecryptImpl(cipherText, key, iv);
        }

        [BlackList]
        public static string Encrypt1(string plainText)
        {
            return EncryptImpl(plainText, key1, iv1);
        }

        [BlackList]
        public static string Decrypt1(string cipherText)
        {
            return DecryptImpl(cipherText, key1, iv1);
        }

        private static string EncryptImpl(string plainText, byte[] key_, byte[] iv_)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key_;
                aes.IV = iv_;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    return System.Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private static string DecryptImpl(string cipherText, byte[] key_, byte[] iv_)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key_;
                aes.IV = iv_;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] bytes = System.Convert.FromBase64String(cipherText);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}