using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    /// <summary>
    /// 表控制器
    /// </summary>
    public class DBTableManager
    {
        private Interface.IDBTable dalDBTable = new DAL.MSSQL.DBTable();
        /// <summary>
        /// 获取表结构
        /// </summary>
        /// <returns></returns>
        public DBTable GetTable(string CID)
        {
            return dalDBTable.GetTable(CID);
        }
        /// <summary>
        /// 获取所有表结构
        /// </summary>
        /// <returns></returns>
        public List<DBTable> GetAllTable()
        {
            return dalDBTable.GetAllTable();
        }
        /// <summary>
        /// 新增表结构
        /// </summary>
        /// <returns></returns>
        public void AddTable(DBTable dbTable)
        {
            dalDBTable.AddTable(dbTable);
        }
        /// <summary>
        /// 更新表结构
        /// </summary>
        /// <returns></returns>
        public void UpdateTable(DBTable dbTable)
        {
            dalDBTable.UpdateTable(dbTable);
        }
        /// <summary>
        /// 删除表结构
        /// </summary>
        /// <returns></returns>
        public void DeleteTable(string CID)
        {
            dalDBTable.DeleteTable(CID);
        }
    }
}
