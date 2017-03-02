using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;

namespace Alteon.Core.Common
{
    public class Security
    {

        /// <summary>
        /// 转MD5值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(String input)
        {
            if (input == null)
                input = "";

            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5");
        }

        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="strIN">需要加密的字符串</param>
        /// <returns>密文</returns>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();


        }

        /// <summary>
        /// (加密)调用 DesEncrypt("金胖子死了", "20111219", "12345678");
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string DesEncrypt(string sourceString, string key, string iv)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                byte[] rgbIV = Encoding.UTF8.GetBytes(iv);

                byte[] inputByteArray = Encoding.UTF8.GetBytes(sourceString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch(Exception ex)
            {
                return sourceString;
            }
        }


        /// <summary>
        /// （解密）调用：DecryptString("C5-0F-7A-F6-77-B1-CB-DE-B8-49-78-64-4E-E7-A4-51", "20111219", "12345678");
        /// </summary>
        /// <param name="sInputString"></param>
        /// <param name="sKey"></param>
        /// <param name="sIV"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString, string sKey, string sIV)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(sKey);
                byte[] rgbIV = Encoding.UTF8.GetBytes(sIV);
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return decryptString;
            }

        }


    }
}
