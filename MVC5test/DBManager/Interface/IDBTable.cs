using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Interface
{
    public interface IDBTable
    {
        /// <summary>
        /// 获取表结构
        /// </summary>
        /// <returns></returns>
        DBTable GetTable(string CID);
        /// <summary>
        /// 获取所有表结构
        /// </summary>
        /// <returns></returns>
        List<DBTable> GetAllTable();
        /// <summary>
        /// 新增表结构
        /// </summary>
        /// <returns></returns>
        void AddTable(DBTable dbTable);
        /// <summary>
        /// 更新表结构
        /// </summary>
        /// <returns></returns>
        void UpdateTable(DBTable dbTable);
        /// <summary>
        /// 删除表结构
        /// </summary>
        /// <returns></returns>
        void DeleteTable(string CID);
    }
}
