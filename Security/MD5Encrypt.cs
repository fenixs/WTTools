using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
namespace WTTools.Security
{
    class MD5Encrypt
    {
        public static byte[] MD5(byte[] data)
        {
            MD5 md = new MD5CryptoServiceProvider();
            return md.ComputeHash(data);
        }

       
        public static string MD5(string text,int code = 0)
        {
            if (code == 0)
            {
                return MD5(text);
            }
            else if (code == 0x10)
            {
                //国内部分论坛常用的加密取值
                return MD5(text).Substring(8, 0x10);
            }
            else
            {
                return MD5(text).Substring(0, code);
            }
        }

        /// <summary>
        /// MD5加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5(string input)
        {
            var md = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(input);
            bs = md.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (var b in bs)
            {
                sb.Append(b.ToString("X2").ToUpper());
            }
            string res = sb.ToString();
            return res;
        }
    }
}
