using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DBEX
    {
        /// <summary>
        /// 對象轉DataTable
        /// </summary>
        public static DataTable ModelToTable(this object obj)
        {
            Type objType = obj.GetType();
            DataTable dt = new DataTable(objType.Name);
            foreach (var propertieInfo in objType.GetPropertiesPGS())
            {
                dt.Columns.Add(propertieInfo.Name, propertieInfo.PropertyType);
            }
            return dt;
        }
        /// <summary>
        /// 獲取可讀寫公共屬性
        /// </summary>
        private static PropertyInfo[] GetPropertiesPGS(this Type objType)
        {
            return objType.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);
        }
        /// <summary>
        /// 對象轉DataTable
        /// </summary>
        public static DataRow ModelToTableRow(this object obj, DataRow dr)
        {
            Type objType = obj.GetType();
            foreach (var propertieInfo in objType.GetPropertiesPGS())
            {
                dr[propertieInfo.Name] = propertieInfo.GetValue(obj);
            }
            return dr;
        }
        /// <summary>
        /// 對象轉DataTable
        /// </summary>
        public static DataTable ListToTable<T>(this List<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            DataTable dt = list[0].ModelToTable();
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();

            }
            return dt;
        }
        /// <summary> 
        /// 大批量插入数据(2000每批次) 
        /// 已采用整体事物控制 
        /// </summary> 
        /// <param name="connString">数据库链接字符串</param> 
        /// <param name="tableName">数据库服务器上目标表名</param> 
        /// <param name="dt">含有和目标数据库表结构完全一致(所包含的字段名完全一致即可)的DataTable</param> 
        public static void BulkCopy(string connString, string tableName, DataTable dt,int timeOut)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.BatchSize = 10000;
                        bulkCopy.BulkCopyTimeout = timeOut;
                        bulkCopy.DestinationTableName = tableName;
                        try
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                            }
                            bulkCopy.WriteToServer(dt);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
        } /// <summary> 
        /// 批量更新数据(每批次5000) 
        /// </summary> 
        /// <param name="connString">数据库链接字符串</param> 
        /// <param name="dt"></param> 
        public static void Update(string connString, DataTable dt, int timeOut)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand comm = conn.CreateCommand();
            comm.CommandTimeout = timeOut;
            comm.CommandType = CommandType.Text;
            SqlDataAdapter adapter = new SqlDataAdapter(comm);
            SqlCommandBuilder commandBulider = new SqlCommandBuilder(adapter);
            commandBulider.ConflictOption = ConflictOption.OverwriteChanges;
            try
            {
                conn.Open();
                //设置批量更新的每次处理条数 
                adapter.UpdateBatchSize = 10000;
                adapter.SelectCommand.Transaction = conn.BeginTransaction();/////////////////开始事务 
                if (dt.ExtendedProperties["SQL"] != null)
                {
                    adapter.SelectCommand.CommandText = dt.ExtendedProperties["SQL"].ToString();
                }
                adapter.Update(dt);
                adapter.SelectCommand.Transaction.Commit();/////提交事务 
            }
            catch (Exception ex)
            {
                if (adapter.SelectCommand != null && adapter.SelectCommand.Transaction != null)
                {
                    adapter.SelectCommand.Transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        } 
    }
}
