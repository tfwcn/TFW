using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DBTable : DBTableBase
    {
        [BWFramework.Common.AttributeEx.DBCol(PKey = true)]
        public string CID { get; set; }
        public string CName { get; set; }
        [BWFramework.Common.AttributeEx.DBCol(CanRead = false, CanWrite = false)]
        public List<DBTableCol> CCols { get; set; }
        public DBTable()
        {
            CCols = new List<DBTableCol>();
        }
    }
}
