using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.Common
{
    public static class Ex
    {
        public static bool IsVoid(this string obj)
        {
            return String.IsNullOrEmpty(obj);
        }
        public static decimal ToDecimal(this double obj)
        {
            return Convert.ToDecimal(obj);
        }
    }
}
