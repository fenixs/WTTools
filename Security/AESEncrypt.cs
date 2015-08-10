using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.IO;

namespace WTTools.Security
{
    class AESEncrypt
    {
        #region "Fields"
        /// <summary>
        /// 加密的Key
        /// </summary>
        private static string _key = "WT_Tools";


        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="src"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Encrypt(string src,string key,string iv)
        {
            byte[] byteskey = Encoding.UTF8.GetBytes(key);
            byte[] bytesiv = Encoding.UTF8.GetBytes(iv);
            byte[] bytessrc = Encoding.UTF8.GetBytes(src);

            string encrypt = null;
            Rijndael aes = Rijndael.Create();

            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream,aes.CreateEncryptor(byteskey,bytesiv),CryptoStreamMode.Write))
                {
                    cStream.Write(bytessrc, 0, bytessrc.Length);
                    cStream.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(mStream.ToArray());
                }
            }
            aes.Clear();
            return encrypt;
        }

        /// <summary>
        /// aes加密
        /// </summary>
        /// <param name="src"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string src, string key)
        {
            return Encrypt(src, key, key);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Encrypt(string src)
        {
            return Encrypt(src, _key, _key);
        }

        /// <summary>
        /// aes解密
        /// </summary>
        /// <param name="src"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Decrypt(string src,string key,string iv)
        {
            var byteskey = Encoding.UTF8.GetBytes(key);
            var bytesiv = Encoding.UTF8.GetBytes(iv);
            var bytessrc = Convert.FromBase64String(src);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();

            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(byteskey, bytesiv), CryptoStreamMode.Write))
                {
                    cStream.Write(bytessrc, 0, bytessrc.Length);
                    cStream.FlushFinalBlock();
                    decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
            aes.Clear();
            return decrypt;
        }

        /// <summary>
        /// aes解密
        /// </summary>
        /// <param name="src"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string src,string key)
        {
            return Decrypt(src, key, key);
        }

        /// <summary>
        /// aes解密
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Decrypt(string src)
        {
            return Decrypt(src, _key, _key);
        }
        #endregion
    }
}
