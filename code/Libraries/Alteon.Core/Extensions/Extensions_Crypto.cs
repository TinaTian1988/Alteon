using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Alteon.Core.Extensions
{
    /// <summary>
    /// 加密类扩展
    /// </summary>
    public static class Extensions_Crypto
    {
        /// <summary>
        /// 生成哈希密码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="passwordFormat"></param>
        /// <returns></returns>
        public static string SetHashPassword(this string data, string passwordFormat = "SHA1")
        {
            if (true == data.AsNullOrWhiteSpace()) return "";

            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(data, passwordFormat);
        }

        #region DES加密解密
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <param name="encryptIV">初始化定向</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(this string encryptString, string encryptKey, string encryptIV)
        {
            if (string.IsNullOrEmpty(encryptString)) return string.Empty;

            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = Encoding.UTF8.GetBytes(encryptIV.Substring(0, 8));
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /**/
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <param name="decryptIV">初始化定向</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(this string decryptString, string decryptKey, string decryptIV)
        {
            if (string.IsNullOrEmpty(decryptString)) return string.Empty;

            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
            byte[] rgbIV = Encoding.UTF8.GetBytes(decryptIV.Substring(0, 8));
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        #endregion
    }
}
