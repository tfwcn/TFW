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
                //Console.WriteLine(DateTime.Now);
                //List<T1> list = new List<T1>();
                //for (int i = 0; i < 1000000; i++)
                //{
                //    T1 t1 = new T1();
                //    t1.fid = Guid.NewGuid().ToString();
                //    t1.fremark = Guid.NewGuid().ToString();
                //    list.Add(t1);
                //}
                //DataTable dt = list.ListToDataTable(null, true);
                //using (SqlDataAdapter da = new SqlDataAdapter())
                //{
                //    SqlTransaction t = conn.BeginTransaction();
                //    da.InsertCommand = new SqlCommand("insert into T1(fid,fremark) values(@fid,@fremark)", conn, t);
                //    da.InsertCommand.CommandTimeout = 3600;//秒
                //    da.InsertCommand.Parameters.Add("@fid", SqlDbType.Char, 36, "fid");
                //    da.InsertCommand.Parameters.Add("@fremark", SqlDbType.Char, 36, "fremark");
                //    da.InsertCommand.UpdatedRowSource = UpdateRowSource.None;//批量更新必须
                //    da.UpdateBatchSize = 0;//批量更新最大值
                //    dt.SetRowsAdd();
                //    da.Update(dt);
                //    t.Commit();
                //}
                //Console.WriteLine(DateTime.Now);
                #endregion
                #region Write2
                //Console.WriteLine(DateTime.Now);
                //List<T1> list = new List<T1>();
                //for (int i = 0; i < 1000000; i++)
                //{
                //    T1 t1 = new T1();
                //    t1.fid = Guid.NewGuid().ToString();
                //    t1.fremark = Guid.NewGuid().ToString();
                //    list.Add(t1);
                //}
                //DataTable dt = list.ListToDataTable(null, true);
                //using (SqlTransaction t = conn.BeginTransaction())
                //{
                //    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, t))
                //    {
                //        bulkCopy.BatchSize = 10000;
                //        bulkCopy.BulkCopyTimeout = 300;
                //        bulkCopy.DestinationTableName = "T1";
                //        bulkCopy.WriteToServer(dt);
                //    }
                //    t.Commit();
                //}
                //Console.WriteLine(DateTime.Now);
                #endregion
                #region Update
                //Console.WriteLine(DateTime.Now);
                //DataTable dt = new DataTable();
                //using (SqlCommand com = new SqlCommand("select * from T1", conn))
                //{
                //    using (SqlDataAdapter da = new SqlDataAdapter(com))
                //    {
                //        da.Fill(dt);
                //    }
                //}
                //Console.WriteLine(DateTime.Now);
                //List<T1> list = dt.DataTableToList<T1>(true, null);
                //list.ForEach(m => m.fremark = Guid.NewGuid().ToString());
                //DataTable dt2 = list.ListToDataTable(null, true);
                //using (SqlDataAdapter da = new SqlDataAdapter())
                //{
                //    SqlTransaction t = conn.BeginTransaction();
                //    da.UpdateCommand = new SqlCommand("update T1 set fremark=@fremark where fid=@fid", conn, t);
                //    da.UpdateCommand.CommandTimeout = 3600;//秒
                //    da.UpdateCommand.Parameters.Add("@fid", SqlDbType.Char, 36, "fid");
                //    da.UpdateCommand.Parameters.Add("@fremark", SqlDbType.Char, 36, "fremark");
                //    da.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;//批量更新必须
                //    da.UpdateBatchSize = 0;//批量更新最大值
                //    dt2.SetRowsUpdate();
                //    da.Update(dt2);
                //    t.Commit();
                //}
                //Console.WriteLine(DateTime.Now);
                #endregion
                #region Delete
                //Console.WriteLine(DateTime.Now);
                //DataTable dt = new DataTable();
                //using (SqlCommand com = new SqlCommand("select top 10 * from T1", conn))
                //{
                //    using (SqlDataAdapter da = new SqlDataAdapter(com))
                //    {
                //        da.Fill(dt);
                //    }
                //}
                //Console.WriteLine(DateTime.Now);
                //List<T1> list = dt.DataTableToList<T1>(true, null);
                //list.ForEach(m => m.fremark = Guid.NewGuid().ToString());
                //DataTable dt2 = list.ListToDataTable(null, true);
                //using (SqlDataAdapter da = new SqlDataAdapter())
                //{
                //    SqlTransaction t = conn.BeginTransaction();
                //    da.DeleteCommand = new SqlCommand("delete from T1 where fid=@fid", conn, t);
                //    da.DeleteCommand.CommandTimeout = 3600;//秒
                //    da.DeleteCommand.Parameters.Add("@fid", SqlDbType.Char, 36, "fid");
                //    da.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;//批量更新必须
                //    da.UpdateBatchSize = 0;//批量更新最大值
                //    dt2.SetRowsDelete();
                //    da.Update(dt2);
                //    t.Commit();
                //}
                //Console.WriteLine(DateTime.Now);
                #endregion
                conn.Close();
            }

            Console.WriteLine(DateTime.Now);
        }
    }
}
