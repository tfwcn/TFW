using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWFramework.Common.AttributeEx;

namespace CodeHelper
{
    public class T1
    {
        [DBCol(PKey=true,CanRead=true,CanWrite=true)]
        public string fid { get; set; }
        public string fremark { get; set; }
    }
}
