using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class EncrytionExtension
    {
        private static byte[] encryptKey = Encoding.UTF8.GetBytes("Xjd9rBGSofLk5nOZR3EN4HUTIz6DxY1e");

        private static byte[] encryptIV = Encoding.UTF8.GetBytes("0TlZ74G2wBPrgH8A");

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plain"></param>
        /// <returns></returns>
        public static string Encrypt(this string plain)
        {
            if (plain.IsEmpty())
                return string.Empty;

            using (Aes aes = Aes.Create())
            {
                aes.Key = encryptKey;
                aes.IV = encryptIV;

                // 设置填充模式为 PKCS#7  
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] data = Encoding.UTF8.GetBytes(plain);
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }


        public static string Decrypt(this string plain)
        {
            if (plain.IsEmpty())
                return string.Empty;


            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = encryptKey;
                    aes.IV = encryptIV;

                    // 设置填充模式为 PKCS#7  
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    byte[] encryptedBytes = Convert.FromBase64String(plain);
                    using (MemoryStream ms = new MemoryStream(encryptedBytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

        }


        public static string Md5Hash(this string plain)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(plain);

                byte[] hash = md5.ComputeHash(data);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }
}
