using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    /// <summary>
    /// 表关系地图
    /// </summary>
    public class DBMap
    {
        /// <summary>
        /// 主键表
        /// </summary>
        public string PKTable { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string PKCol { get; set; }
        /// <summary>
        /// 外键表
        /// </summary>
        public string FKTable { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public string FKCol { get; set; }
    }
}
