using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeHelper
{
    public class MRow
    {
        public string 字段 { get; set; }
        public string 数据类型 { get; set; }
        public int 长度 { get; set; }
        public int 小数位 { get; set; }
        public bool 允许空值 { get; set; }
        public bool 主键 { get; set; }
        public string 默认值 { get; set; }
        public string 说明 { get; set; }
    }
}
