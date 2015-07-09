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
                DataColumn col = dt.Columns.Add(propertieInfo.Name, (propertieInfo.PropertyType.IsGenericType && propertieInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? propertieInfo.PropertyType.GetGenericArguments()[0] : propertieInfo.PropertyType);
                DBEX.DBColAttribute attribute = Attribute.GetCustomAttribute(propertieInfo, typeof(DBEX.DBColAttribute)) as DBEX.DBColAttribute;
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
        /// 對象轉DataTable(含值)
        /// </summary>
        public static DataTable ModelToDataTableHasValue(this object obj, bool? dbCanRead, bool? dbCanWrite)
        {
            Type objType = obj.GetType();
            DataTable dt = new DataTable(objType.Name);
            List<DataColumn> pkList = new List<DataColumn>();
            foreach (var propertieInfo in objType.GetPropertiesPGS(dbCanRead, dbCanWrite))
            {
                DataColumn col = dt.Columns.Add(propertieInfo.Name, (propertieInfo.PropertyType.IsGenericType && propertieInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? propertieInfo.PropertyType.GetGenericArguments()[0] : propertieInfo.PropertyType);
                DBEX.DBColAttribute attribute = Attribute.GetCustomAttribute(propertieInfo, typeof(DBEX.DBColAttribute)) as DBEX.DBColAttribute;
                if (attribute != null && attribute.PKey == true)
                {
                    pkList.Add(col);
                }
            }
            if (pkList.Count > 0)
            {
                dt.PrimaryKey = pkList.ToArray();
            }
            DataRow dr= dt.NewRow();
            obj.ModelToDataRow(dr, objType.GetPropertiesPGS(dbCanRead, dbCanWrite));
            dt.Rows.Add(dr);
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
                DBEX.DBColAttribute attribute = Attribute.GetCustomAttribute(property, typeof(DBEX.DBColAttribute)) as DBEX.DBColAttribute;
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
                dr[propertieInfo.Name] = propertieInfo.GetValue(obj) == null ? DBNull.Value : propertieInfo.GetValue(obj);
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
        public static T DataRowToModel<T>(this DataRow dr, bool? dbCanRead, bool? dbCanWrite) where T : new()
        {
            PropertyInfo[] properties = typeof(T).GetPropertiesPGS(dbCanRead, dbCanWrite);
            return dr.DataRowToModel<T>(properties);
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
            dt.AcceptChanges();
            foreach (DataRow row in dt.Rows)
            {
                row.SetAdded();
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
            dt.AcceptChanges();
            foreach (DataRow row in dt.Rows)
            {
                row.SetModified();
            }
        }
        /// <summary>
        /// 设置行状态为刪除
        /// </summary>
        public static void SetRowsDelete(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }
            dt.AcceptChanges();
            foreach (DataRow row in dt.Rows)
            {
                row.Delete();
            }
        }
    }
}
