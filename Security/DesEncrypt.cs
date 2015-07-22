using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace WTTools.Security
{
    class DesEncrypt
    {

        #region "fields"
        /// <summary>
        /// 加密的Key
        /// </summary>
        private string _Key = "WT_Tools";

        /// <summary>
        /// 加密的key
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }

        #endregion

        #region "构造 析构"
        private DesEncrypt()
        {

        }

        ~DesEncrypt()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region "Encrypt"
        
        
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public string Encrypt(string input,out string key,out string iv)
        {
            key = "";
            iv = "";
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(input);
            provider.GenerateIV();
            provider.GenerateKey();
            iv = new UnicodeEncoding().GetString(provider.IV);
            key = new UnicodeEncoding().GetString(provider.Key);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder sb = new StringBuilder();
            var sarray = stream.ToArray();
            foreach (var item in sarray)
            {
                sb.AppendFormat("{0:X2}", item);
            }
            stream.Close();
            stream2.Close();
            return sb.ToString();
        }

        public byte[] MakeMD5(byte[] original)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(original);
            return buffer;
        }

        public byte[] Encrypt(byte[] original,byte[] key)
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider
            {
                Key = this.MakeMD5(key),
                Mode = CipherMode.ECB
            };

            return provider.CreateEncryptor().TransformFinalBlock(original,0,original.Length);
        }

        public string Encrypt(string input,string skey)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(input);
            //var bytestemp = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(skey, "md5").Substring(0, 8));
            var bytestemp = Encoding.ASCII.GetBytes(MD5Encrypt.MD5(skey,8));
            provider.Key = bytestemp;
            provider.IV = bytestemp;
            StringBuilder sb = new StringBuilder();
            
            using(MemoryStream stream = new MemoryStream())
            {
                using(CryptoStream stream2 = new CryptoStream(stream,provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    stream2.Write(bytes, 0, bytes.Length);
                    stream2.FlushFinalBlock();
                    var sarray = stream.ToArray();
                    foreach (var item in sarray)
                    {
                        sb.AppendFormat("{0:X2}", item);
                    }
                }
            }
            return sb.ToString();

        }

        public string Encrypt(string input)
        {
            return Encrypt(input, this._Key);
        }

        #endregion

        #region "Decrypt"

        public string Decrypt(string input)
        {
            return Decrypt(input, this._Key);
        }

        public string Decrypt(string input,string key)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            int num = input.Length / 2;
            byte[] buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                int tmp = Convert.ToInt32(input.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)tmp;
            }
            //var bytestemp = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
            var bytestemp = Encoding.ASCII.GetBytes(MD5Encrypt.MD5(key,8));
            provider.IV = bytestemp;
            provider.Key = bytestemp;
            using(MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    stream2.Write(buffer, 0, buffer.Length);
                    stream2.FlushFinalBlock();
                    return Encoding.Default.GetString(stream.ToArray());
                }
            }
        }

        public string Decrypt(string input,string iv,string key)
        {
            byte[] byteskey = new UnicodeEncoding().GetBytes(key);
            byte[] bytesiv = new UnicodeEncoding().GetBytes(iv);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            int num = input.Length / 2;
            byte[] buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                int tmp = Convert.ToInt32(input.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)tmp;
            }
            provider.Key = byteskey;
            provider.IV = bytesiv;
            using(MemoryStream stream = new MemoryStream())
            {
                using(CryptoStream stream2 = new CryptoStream(stream,provider.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    stream2.Write(buffer, 0, buffer.Length);
                    stream2.FlushFinalBlock();
                    return Encoding.Default.GetString(stream.ToArray());
                }
            }

        }

        #endregion

    }




}
