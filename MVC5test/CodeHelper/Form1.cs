using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeHelper
{
    public partial class Form1 : Form
    {
        List<MRow> rows = new List<MRow>();
        BindingSource bs = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bs.DataSource = rows;
            dataGridView1.DataSource = bs;
        }
        public void DataGridViewEnableCopy(DataGridView p_Data)
        {
            Clipboard.SetData(DataFormats.Text, p_Data.GetClipboardContent());
        }
        public void DataGirdViewCellPaste(DataGridView p_Data)
        {
            try
            {
                // 获取剪切板的内容，并按行分割
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText))
                    return;
                string[] lines = pasteText.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line.Trim()))
                        continue;
                    // 按 Tab 分割数据
                    string[] vals = line.Split('\t');
                    MRow row = new MRow();
                    row.字段 = vals[0].Trim();
                    row.数据类型 = vals[1].Trim();
                    row.长度 = Convert.ToInt32(vals[2].Trim());
                    row.小数位 = Convert.ToInt32(vals[3].Trim());
                    if (vals[4].Trim() != "")
                        row.允许空值 = Convert.ToBoolean(vals[4].Trim());
                    else
                        row.允许空值 = true;
                    if (vals[5].Trim() != "")
                        row.主键 = Convert.ToBoolean(vals[5].Trim());
                    row.默认值 = vals[6].Trim();
                    row.说明 = vals[7].Trim();
                    rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                rows.Clear();
            }
            bs.ResetBindings(false);
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control && e.KeyChar == 3)
            {
                if (sender != null && sender.GetType() == typeof(DataGridView))
                    DataGridViewEnableCopy((DataGridView)sender);
            }
            if (Control.ModifierKeys == Keys.Control && e.KeyChar == 22)
            {
                if (sender != null && sender.GetType() == typeof(DataGridView))
                    DataGirdViewCellPaste((DataGridView)sender);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string code = @"
CREATE TABLE {0}
(
  {1}
  fcuser text NOT NULL, -- 创建人
  fctime timestamp without time zone NOT NULL DEFAULT now(), -- 创建时间
  fuuser text NOT NULL, -- 更新人
  futime timestamp without time zone NOT NULL DEFAULT now(), -- 更新时间
  flog text NOT NULL, -- 日志
  frowversion integer NOT NULL, -- 更新版本号
  {2}
)
WITH (
  OIDS=FALSE
);
ALTER TABLE {0}
  OWNER TO postgres;
{3}
COMMENT ON COLUMN {0}.fcuser IS '创建人';
COMMENT ON COLUMN {0}.fctime IS '创建时间';
COMMENT ON COLUMN {0}.fuuser IS '更新人';
COMMENT ON COLUMN {0}.futime IS '更新时间';
COMMENT ON COLUMN {0}.flog IS '日志';
COMMENT ON COLUMN {0}.frowversion IS '更新版本号';
";
            code = code.Replace("\n", "\r\n");
            code = code.Replace("\r\n\n", "\r\n");
            string tableName = textBox2.Text.Trim();
            {
                string str1 = "";
                foreach (var row in rows)
                {
                    str1 += string.Format(" {0} {1}{2} {3} {4}, --{5}\r\n",
                        row.字段,
                        row.数据类型,
                        row.长度 == 0 ? "" : "(" + row.长度 + (row.小数位 == 0 ? "" : "," + row.小数位) + ")",
                        row.允许空值 == true ? "" : "NOT NULL",
                        row.默认值 == "" ? "" : "DEFAULT " + row.默认值 + "::" + row.数据类型,
                        row.说明);
                }
                string str2 = "";
                if (rows.Exists(m => m.主键))
                {
                    str2 = " CONSTRAINT " + tableName + "_pkey PRIMARY KEY (" + rows.Find(m => m.主键).字段 + ") \r\n";
                }
                string str3 = "";
                foreach (var row in rows)
                {
                    str3 += string.Format(" COMMENT ON COLUMN {0}.{1} IS '{2}';\r\n",
                        tableName,
                        row.字段,
                        row.说明);
                }

                List<object> objs = new List<object>();
                objs.Add(tableName);
                objs.Add(str1);
                objs.Add(str2);
                objs.Add(str3);
                code = string.Format(code, objs.ToArray());
                textBox1.Text = code;
            }

            string code1 = @"

--============================


";
            code1 = code1.Replace("\n", "\r\n");
            textBox1.Text += code1;

            string code2 = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
namespace JCloud.Model
{{
    [Common.Table(TableName = ""{0}"")]
    public class {0}info:BaseModel
    {{
        {1}
    }}
}}
";
            //code2 = code2.Replace("\n", "\r\n");
            {
                string str1 = "";
                foreach (var row in rows)
                {
                    string 数据类型 = "";
                    switch (row.数据类型)
                    {
                        case "text":
                        case "character":
                        case "character varying":
                            数据类型 = "string";
                            break;
                        case "bit":
                        case "boolean":
                            数据类型 = "bool";
                            break;
                        case "timestamp with time zone":
                        case "timestamp without time zone":
                            数据类型 = "DateTime";
                            break;
                        case "integer":
                            数据类型 = "int";
                            break;
                        case "numeric":
                            数据类型 = "decimal";
                            break;
                        default:
                            throw new Exception("类型错误！");
                    }
                    str1 += string.Format(@"
        /// <summary>
        /// {0}
        /// </summary>
        public {2} {1} {{ get; set; }}
",
                        row.说明,
                        row.字段,
                        数据类型);
                }
                //str1 = str1.Replace("\n", "\r\n");

                List<object> objs = new List<object>();
                objs.Add(tableName);
                objs.Add(str1);
                code2 = string.Format(code2, objs.ToArray());
                textBox1.Text += code2;
            }
        }
    }
}
