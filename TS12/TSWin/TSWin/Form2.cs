using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSWin
{
    public partial class Form2 : Form
    {
        public class TData
        {
            //public int RN { get; set; }
            public string TNAME { get; set; }
        }
        private List<TData> listData = new List<TData>();
        public Form2()
        {
            InitializeComponent();
            for (int i = 0; i < 100; i++)
            {
                listData.Add(new TData() { TNAME = i.ToString() });
            }
            gridView1.DataSourceChanged += gridView1_DataSourceChanged;
            gridControl1.DataSource = listData;
            this.gridView1.IndicatorWidth = 40;//显示行的序号
            //gridView1.SetRowCellValue(10, "RN", 100);
            //var row= gridView1.GetDataRow(10);
            //row["RN"] = 100;
            var col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.Caption = "行號";
            col.FieldName = "RN";
            col.Name = "RN";
            col.Visible = true;
            col.VisibleIndex = 0;
            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            gridView1.Columns.Insert(0,col);
        }

        void gridView1_DataSourceChanged(object sender, EventArgs e)
        {
            for(int i=0;i<gridView1.RowCount;i++)
            {
                gridView1.SetRowCellValue(i, "RN", (i + 1));
                gridView1.UpdateCurrentRow();
                //gridView1.Columns["RN"].UnboundExpression = (e.RowHandle + 1).ToString();
                //gridView1.Columns["RN"].UnboundType = DevExpress.Data.UnboundColumnType.Integer;
                //var row = gridView1.e;
                //row["RN"] = 100;
                //e.Info.DisplayText = (e.RowHandle + 1).ToString();
                //e.Handled = true;
            }
        }
    }
}
