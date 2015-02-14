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
        public class DBColAttribute : Attribute
        {
            private bool canRead = true;
            private bool canWrite = true;
            public bool CanRead { get { return canRead; } set { canRead = value; } }
            public bool CanWrite { get { return canWrite; } set { canWrite = value; } }
            public bool PKey { get; set; }
        }
        /// <summary>
        /// 對象轉DataTable
        /// </summary>
        public static DataTable ModelToDataTable(this object obj, bool? dbCanRead, bool? dbCanWrite)
        {
            Type objType = obj.GetType();
            DataTable dt = new DataTable(objType.Name);
            List<DataColumn> pkList = new List<DataColumn>();
            foreach (var propertieInfo in objType.GetPropertiesPGS(dbCanRead, dbCanWrite))
            {
                DataColumn col = dt.Columns.Add(propertieInfo.Name, propertieInfo.PropertyType);
                DBEX.DBColAttribute attribute = Attribute.GetCustomAttribute(objType, typeof(DBEX.DBColAttribute)) as DBEX.DBColAttribute;
                if (attribute != null && attribute.PKey == true)
                {
                    pkList.Add(col);
                }
            }
            if (pkList.Count > 0)
            {
                dt.PrimaryKey = pkList.ToArray();
            }
            return dt;
        }
        /// <summary>
        /// 獲取可讀寫公共屬性
        /// </summary>
        private static PropertyInfo[] GetPropertiesPGS(this Type objType, bool? dbCanRead, bool? dbCanWrite)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            foreach (var property in objType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                DBEX.DBColAttribute attribute = Attribute.GetCustomAttribute(objType, typeof(DBEX.DBColAttribute)) as DBEX.DBColAttribute;
                if (attribute == null || ((dbCanRead == null || attribute.CanRead == dbCanRead) && (dbCanWrite == null || attribute.CanWrite == dbCanWrite)))
                {
                    properties.Add(property);
                }
            }
            return properties.ToArray();
        }
        /// <summary>
        /// 對象轉DataRow
        /// </summary>
        public static DataRow ModelToDataRow(this object obj, DataRow dr, PropertyInfo[] properties)
        {
            Type objType = obj.GetType();
            foreach (var propertieInfo in properties)
            {
                dr[propertieInfo.Name] = propertieInfo.GetValue(obj);
            }
            return dr;
        }
        /// <summary>
        /// 對象轉DataTable
        /// </summary>
        public static DataTable ListToDataTable<T>(this List<T> list, bool? dbCanRead, bool? dbCanWrite) where T : new()
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            T t = list[0];
            DataTable dt = t.ModelToDataTable(dbCanRead, dbCanWrite);
            Type objType = t.GetType();
            PropertyInfo[] properties = objType.GetPropertiesPGS(null, null);
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                item.ModelToDataRow(dr, properties);
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// DataRow轉對象
        /// </summary>
        public static T DataRowToModel<T>(this DataRow dr, PropertyInfo[] properties) where T : new()
        {
            T t = new T();
            foreach (var propertieInfo in properties)
            {
                propertieInfo.SetValue(t, dr[propertieInfo.Name]);
            }
            return t;
        }
        /// <summary>
        /// DataTable轉對象
        /// </summary>
        public static List<T> DataTableToList<T>(this DataTable dt, bool? dbCanRead, bool? dbCanWrite) where T : new()
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            List<T> list = new List<T>();
            PropertyInfo[] properties = typeof(T).GetPropertiesPGS(dbCanRead, dbCanWrite);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(row.DataRowToModel<T>(properties));
            }
            return list;
        }
        /// <summary>
        /// 设置行状态为新增
        /// </summary>
        public static void SetRowsAdd(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Unchanged)
                {
                    row.SetAdded();
                }
            }
        }
        /// <summary>
        /// 设置行状态为更新
        /// </summary>
        public static void SetRowsUpdate(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Unchanged)
                {
                    row.SetModified();
                }
            }
        }
        /// <summary> 
        /// 大批量插入数据(2000每批次) 
        /// 已采用整体事物控制 
        /// </summary> 
        /// <param name="connString">数据库链接字符串</param> 
        /// <param name="tableName">数据库服务器上目标表名</param> 
        /// <param name="dt">含有和目标数据库表结构完全一致(所包含的字段名完全一致即可)的DataTable</param> 
        public static void BulkCopy(string connString, string tableName, DataTable dt, int timeOut)
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
