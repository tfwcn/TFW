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
using DBManager;
using BWFramework.Common;

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
            /*using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=DBTest;Persist Security Info=True;User ID=sa;Password=sa123"))
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
                //DataTable dt = list.ListToDataTable<T1>(null, true);
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
            }//*/
            #region NewTable
            /*DBTable t = new DBTable();
            t.CID = Guid.NewGuid().ToString();
            t.CGID = Guid.NewGuid().ToString();
            t.CName = "A表";
            t.CCUser = "SA";
            t.CUUser = "SA";
            DBTableCol c1 = new DBTableCol();
            c1.CID = Guid.NewGuid().ToString();
            c1.CGID = t.CGID;
            c1.CDBTableID = t.CID;
            c1.CPK = true;
            c1.CType = DBTableCol.ColType.英文;
            c1.CSize = 36;
            c1.CName = "A主键";
            c1.CNotNULL = true;
            c1.CCUser = "SA";
            c1.CUUser = "SA";
            t.CCols.Add(c1);
            c1 = new DBTableCol();
            c1.CID = Guid.NewGuid().ToString();
            c1.CGID = t.CGID;
            c1.CDBTableID = t.CID;
            c1.CIX = DBTableCol.IXType.唯一索引;
            c1.CType = DBTableCol.ColType.文本;
            c1.CSize = 50;
            c1.CName = "A唯一索引";
            c1.CDefaultValue = "aa中文";
            c1.CCUser = "SA";
            c1.CUUser = "SA";
            t.CCols.Add(c1);
            c1 = new DBTableCol();
            c1.CID = Guid.NewGuid().ToString();
            c1.CGID = t.CGID;
            c1.CDBTableID = t.CID;
            c1.CIX = DBTableCol.IXType.普通索引;
            c1.CType = DBTableCol.ColType.数字;
            c1.CSize = 8;
            c1.CDecimalsSize = 4;
            c1.CName = "A普通索引";
            c1.CDefaultValue = "1";
            c1.CCUser = "SA";
            c1.CUUser = "SA";
            t.CCols.Add(c1);
            DBManager.DBTableManager dbTableManager = new DBTableManager();
            dbTableManager.AddTable(t);//*/
            #endregion
            //string s = Common.Encryption.Decrypt("PDOwkfEV0Zu4D+12loQkZ7Couzi31WMRClZW0vsdLCx5xFlfq8DZxA0U7Ziv8tFXPJ9v8Ba8YSE3ycdcsMnOJ0LLGKEwwQzjyWcS0BgUj5K8VitaPhuZj/04E1PSCEfb", "GOLDSALES");
            //s = Common.Encryption.Encrypt("Data Source=192.168.4.18;Initial Catalog=GOLDSALES;User ID=3247;Password=511400a", "GOLDSALES");
            //欲进行md5加密的字符串  
            string test = "123abc";
            /*
            //获取加密服务  
            System.Security.Cryptography.MD5CryptoServiceProvider md5CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();  
         
            //获取要加密的字段，并转化为Byte[]数组  
            byte[] testEncrypt = System.Text.Encoding.Unicode.GetBytes(test);  
  
            //加密Byte[]数组  
            byte[] resultEncrypt = md5CSP.ComputeHash(testEncrypt);  
  
            //将加密后的数组转化为字段(普通加密)  
            string testResult = System.Text.Encoding.Unicode.GetString(resultEncrypt);
            //*/
            string testResult = GetMD5(test);
            Console.WriteLine(DateTime.Now);
        }
        public string GetRandomNum(string value, string key, int size, int index)
        {
            string retValue = "";
            return retValue;
        }
        public static string GetMD5(string sDataIn)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BWFramework.BLL.TUser bllTUser = new BWFramework.BLL.TUser();
            BWFramework.BLL.TUserInfo bllTUserInfo = new BWFramework.BLL.TUserInfo();
            //List<BWFramework.Model.TUser> tmpListTUser = new List<BWFramework.Model.TUser>();
            //List<BWFramework.Model.TUserInfo> tmpListTUserInfo = new List<BWFramework.Model.TUserInfo>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    BWFramework.Model.TUser tmpTUser = new BWFramework.Model.TUser();
            //    tmpTUser.CID = Guid.NewGuid().ToString();
            //    tmpTUser.CLoginNo = "test" + i;
            //    tmpTUser.CPassword = "test" + i;
            //    tmpTUser.CName = "新用户" + i;
            //    tmpListTUser.Add(tmpTUser);

            //    BWFramework.Model.TUserInfo tmpTUserInfo = new BWFramework.Model.TUserInfo();
            //    tmpTUserInfo.CID = Guid.NewGuid().ToString();
            //    tmpTUserInfo.CUserID = tmpTUser.CID;
            //    tmpTUserInfo.CSex = new Random().Next(3);
            //    tmpTUserInfo.CMoney = i * new Random().NextDouble().ToDecimal() * 1000;
            //    tmpListTUserInfo.Add(tmpTUserInfo);
            //    //bllTUser.Register(tmpTUser, tmpTUserInfo);
            //}
            //bllTUser.Register(tmpListTUser, tmpListTUserInfo);

            BWFramework.Model.TUser tmpTUser = bllTUser.GetModelByLoginNo("test1");
            BWFramework.Model.TUserInfo tmpTUserInfo = bllTUserInfo.GetModelByCUserID(tmpTUser.CID);
            tmpTUserInfo.CSex = new Random().Next(3);
            tmpTUserInfo.CMoney = new Random().NextDouble().ToDecimal() * 1000;
            bllTUserInfo.Update(tmpTUserInfo);
        }
    }
}
