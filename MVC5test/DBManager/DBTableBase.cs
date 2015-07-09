using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DBTableBase
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CCTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CCUser { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime CUTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string CUUser { get; set; }
        /// <summary>
        /// 用户组
        /// </summary>
        public string CGID { get; set; }
        /// <summary>
        /// 数据版本号
        /// </summary>
        public int CVersion { get; set; }
    }
}
