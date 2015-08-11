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
    public class AESEncrypt
    {
        #region "Fields"
        /// <summary>
        /// 加密的Key
        /// </summary>
        private static string _key = "WT_ToolsWT_ToolsWT_ToolsWT_Tools";

        #endregion


        #region "ECB方法加密"


        /// <summary>
        /// AES加密 ECB
        /// 256位
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Encrypt(string src,string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = _key;
            }
            else
            {
                if (key.Length > 32)
                {
                    key = key.Substring(0, 32);
                }
                else if (key.Length < 32)
                {
                    key = key.PadLeft(32, ' ');
                }
            }

            var byteskey = Encoding.UTF8.GetBytes(key);
            var bytessrc = Encoding.UTF8.GetBytes(src);

            Aes aes = AesManaged.Create();
            aes.Key = byteskey;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            var ct = aes.CreateEncryptor();
            var res = ct.TransformFinalBlock(bytessrc, 0, bytessrc.Length);

            return Convert.ToBase64String(res);
        }


        public static string Decrypt(string src,string key)
        {
            if(string.IsNullOrEmpty(key))
            {
                key = _key;
            }
            else
            {
                if(key.Length>32)
                {
                    key = key.Substring(0, 32);
                }
                else if(key.Length<32)
                {
                    key = key.PadLeft(32, ' ');
                }
            }
            var byteskey = Encoding.UTF8.GetBytes(key);
            var bytessrc = Convert.FromBase64String(src);

            Aes aes = AesManaged.Create();
            aes.Key = byteskey;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            var ct = aes.CreateDecryptor();
            var res = ct.TransformFinalBlock(bytessrc, 0, bytessrc.Length);

            return Encoding.UTF8.GetString(res);

        }

        

        #endregion
        #region "废弃"

        ///// <summary>
        ///// AES加密
        ///// </summary>
        ///// <param name="src"></param>
        ///// <param name="key"></param>
        ///// <param name="iv"></param>
        ///// <returns></returns>
        //public static string Encrypt(string src,string key,string iv)
        //{

        //    byte[] byteskey = Encoding.UTF8.GetBytes(key);
        //    byte[] bytesiv = Encoding.UTF8.GetBytes(iv);
        //    byte[] bytessrc = Encoding.UTF8.GetBytes(src);

        //    string encrypt = null;
        //    Rijndael aes = Rijndael.Create();

        //    using (MemoryStream mStream = new MemoryStream())
        //    {
        //        using (CryptoStream cStream = new CryptoStream(mStream,aes.CreateEncryptor(byteskey,bytesiv),CryptoStreamMode.Write))
        //        {
        //            cStream.Write(bytessrc, 0, bytessrc.Length);
        //            cStream.FlushFinalBlock();
        //            encrypt = Convert.ToBase64String(mStream.ToArray());
        //        }
        //    }
        //    aes.Clear();
        //    return encrypt;
        //}

        ///// <summary>
        ///// aes加密
        ///// </summary>
        ///// <param name="src"></param>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string Encrypt(string src, string key)
        //{
        //    return Encrypt(src, key, key);
        //}

        ///// <summary>
        ///// AES加密
        ///// </summary>
        ///// <param name="src"></param>
        ///// <returns></returns>
        //public static string Encrypt(string src)
        //{
        //    return Encrypt(src, _key, _key);
        //}

        ///// <summary>
        ///// aes解密
        ///// </summary>
        ///// <param name="src"></param>
        ///// <param name="key"></param>
        ///// <param name="iv"></param>
        ///// <returns></returns>
        //public static string Decrypt(string src,string key,string iv)
        //{
        //    var byteskey = Encoding.UTF8.GetBytes(key);
        //    var bytesiv = Encoding.UTF8.GetBytes(iv);
        //    var bytessrc = Convert.FromBase64String(src);

        //    string decrypt = null;
        //    Rijndael aes = Rijndael.Create();

        //    using (MemoryStream mStream = new MemoryStream())
        //    {
        //        using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(byteskey, bytesiv), CryptoStreamMode.Write))
        //        {
        //            cStream.Write(bytessrc, 0, bytessrc.Length);
        //            cStream.FlushFinalBlock();
        //            decrypt = Encoding.UTF8.GetString(mStream.ToArray());
        //        }
        //    }
        //    aes.Clear();
        //    return decrypt;
        //}

        ///// <summary>
        ///// aes解密
        ///// </summary>
        ///// <param name="src"></param>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string Decrypt(string src,string key)
        //{
        //    return Decrypt(src, key, key);
        //}

        ///// <summary>
        ///// aes解密
        ///// </summary>
        ///// <param name="src"></param>
        ///// <returns></returns>
        //public static string Decrypt(string src)
        //{
        //    return Decrypt(src, _key, _key);
        //}


        #endregion
    }
}
