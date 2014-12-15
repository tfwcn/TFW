using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class GUIDHelper
    {
        private const string BASE_CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_";
        private static uint BCLen = (uint)BASE_CHAR.Length;
        /// <summary>
        /// 16進制字符串转换为段字符
        /// </summary>
        private static string GetLongNo(UInt64 num, int length)
        {
            string str = "";
            while (num > 0)
            {
                int cur = (int)(num % BCLen);
                str = BASE_CHAR[cur] + str;
                num = num / BCLen;
            }

            if (str.Length > length)
            {
                throw new Exception("轉換超長度");
            }

            else
            {
                str = str.PadLeft(length, '0');
            }
            return str;
        }

        /// <summary>
        /// 段字符转换为16進制字符串
        /// </summary>
        private static UInt64 GetLongHex(string strNo)
        {
            UInt64 num = 0;
            for (int i = 0; i < strNo.Length; i++)
            {
                num += (UInt64)BASE_CHAR.IndexOf(strNo[i]) * (UInt64)Math.Pow(BASE_CHAR.Length, strNo.Length - i - 1);
            }
            return num;
        }

        /// <summary>
        /// 壓縮guid
        /// </summary>
        public static string GetGUIDNo(Guid guid)
        {
            string strguid = guid.ToString().Replace("-","");
            ulong guid1 = UInt64.Parse(strguid.Substring(0, 16), System.Globalization.NumberStyles.AllowHexSpecifier);
            ulong guid2 = UInt64.Parse(strguid.Substring(16), System.Globalization.NumberStyles.AllowHexSpecifier);
            string str1 = GetLongNo(guid1, 11);
            string str2 = GetLongNo(guid2, 11);
            return str1 + str2;
        }

        /// <summary>
        /// 解壓guid
        /// </summary>
        public static Guid GetGUID(string guidNo)
        {
            ulong guid1 = GetLongHex(guidNo.Substring(0, 11));
            ulong guid2 = GetLongHex(guidNo.Substring(11));
            string str1 = guid1.ToString("X").PadLeft(16, '0');
            string str2 = guid2.ToString("X").PadLeft(16, '0');
            return new Guid(str1 + str2);
        }
    }
}
