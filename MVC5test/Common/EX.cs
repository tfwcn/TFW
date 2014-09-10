using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class EX
    {
        public static bool IsVoid(this string obj)
        {
            return String.IsNullOrEmpty(obj);
        }
    }
}
