using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TSWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            XtraReport1 rp = new XtraReport1();
            rp.Parameters["TCORP"].Value = "GZ";
            rp.Parameters["TSDATE"].Value = DateTime.Now.ToString("yyyy-MM-dd");
            rp.Parameters["TEDATE"].Value = DateTime.Now.ToString("yyyy-MM-dd");
            rp.Parameters["TSTOCK"].Value = "AA,BB,CC";
            MemoryStream ms=new MemoryStream();
            Properties.Resources.aimg.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            DataSet1 ds = new DataSet1();
            for (int i = 0; i < 3; i++)
            {
                var dr = ds.DataTable1.NewDataTable1Row();
                dr.TID = "a" + i;
                dr.TNAME = "A" + i;
                dr.TQTY = i;
                dr.TIMG = ms.GetBuffer();
                ds.DataTable1.Rows.Add(dr);
            }
            rp.DataSource = ds;
            rp.CreateDocument();
            this.documentViewer1.DocumentSource = rp;
        }
    }
}
