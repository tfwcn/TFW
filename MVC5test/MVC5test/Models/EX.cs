using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVC5test
{
    public static class EX
    {
        public static bool IsVoid(this string obj)
        {
            return String.IsNullOrEmpty(obj);
        }
        public static string GetAttributeHTML(this int obj)
        {
            if ((int)obj == 0)//整形
                return "整形";
            else if ((int)obj == 1)//數字類型
                return "數字類型";
            else if ((int)obj == 2)//字符串
                return "字符串";
            return "";
        }
    }
}
