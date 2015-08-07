using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DBTableCol : DBTableBase
    {
        public enum ColType
        {
            文本 = 0,
            数字 = 1,
            是否 = 2,
            选项 = 3,
            英文 = 4
        }
        public enum IXType
        {
            无索引 = 0,
            唯一索引 = 1,
            普通索引 = 2
        }
        [BWFramework.Common.AttributeEx.DBCol(PKey = true)]
        public string CID { get; set; }
        public string CDBTableID { get; set; }
        public string CName { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public bool CPK { get; set; }
        /// <summary>
        /// 非空
        /// </summary>
        public bool CNotNULL { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string CDefaultValue { get; set; }
        /// <summary>
        /// 索引
        /// </summary>
        public IXType CIX { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public ColType CType { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public int? CSize { get; set; }
        /// <summary>
        /// 小数长度
        /// </summary>
        public int? CDecimalsSize { get; set; }
    }
}
