using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace CodeHelper
{
    public class T1
    {
        [DBEX.DBCol(PKey=true,CanRead=true,CanWrite=true)]
        public string fid { get; set; }
        public string fremark { get; set; }
    }
}
