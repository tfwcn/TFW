using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Common;

namespace CodeHelper
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(DateTime.Now);
            using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=DBTest;Persist Security Info=True;User ID=sa;Password=sa123"))
            {
                conn.Open();
                #region Read
                //Console.WriteLine(DateTime.Now);
                //DataTable dt = new DataTable();
                //using (SqlCommand com = new SqlCommand("select * from T1",conn))
                //{
                //    using (SqlDataAdapter da = new SqlDataAdapter(com))
                //    {
                //        da.Fill(dt);
                //    }
                //}
                //Console.WriteLine(DateTime.Now);
                //List<T1> list = dt.DataTableToList<T1>(true, null);
                #endregion
                #region Write
                Console.WriteLine(DateTime.Now);
                List<T1> list = new List<T1>();
                for (int i = 0; i < 1000000; i++)
                {
                    T1 t1 = new T1();
                    t1.fid = Guid.NewGuid().ToString();
                    t1.fremark = Guid.NewGuid().ToString();
                    list.Add(t1);
                }
                DataTable dt = list.ListToDataTable(null, true);
                using (SqlCommand com = new SqlCommand("INSERT INTO T1(fid,fremark)VALUES(@fid,@fremark)", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        SqlCommandBuilder commandBulider = new SqlCommandBuilder(da);
                        commandBulider.ConflictOption = ConflictOption.OverwriteChanges;
                        //da.InsertCommand = com;
                        da.InsertCommand.UpdatedRowSource = UpdateRowSource.None;//批量更新必须
                        //da.InsertCommand.Parameters.Add("fid", SqlDbType.Char, 36);
                        //da.InsertCommand.Parameters.Add("fremark", SqlDbType.Char, 36);
                        //da.InsertCommand.Parameters["fid"].SourceColumn = "fid";
                        //da.InsertCommand.Parameters["fremark"].SourceColumn = "fremark";
                        da.UpdateBatchSize = 0;//批量更新最大值
                        //dt.SetRowsAdd();
                        da.Update(dt);
                    }
                }
                Console.WriteLine(DateTime.Now);
                #endregion
                conn.Close();
            }
            
            Console.WriteLine(DateTime.Now);
        }
    }
}
