namespace Alteon.Core.Extensions
{
    using System.Net;

    /// <summary>
    /// 长整形数字扩展类
    /// </summary>
    public static class LongExt
    {
        /// <summary>
        /// 把数字转换为IP地址形式的字符串
        /// </summary>
        /// <param name="l">数字形式的IP地址</param>
        public static string ToIP(this long l)
        {
            byte[] b = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                b[3 - i] = (byte)(l >> 8 * i & 255);
            }

            return new IPAddress(b).ToString();
        }
    }
}