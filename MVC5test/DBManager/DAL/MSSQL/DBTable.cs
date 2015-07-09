using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using Common;

namespace DBManager.DAL.MSSQL
{
    class DBTable : DBManager.Interface.IDBTable
    {
        private DBHelper.MSSQLHelper dbHelper = new DBHelper.MSSQLHelper();
        #region IDBTable 成员

        public DBManager.DBTable GetTable(string CID)
        {
            string sqlStr = " select * from DBTable where CID=@CID ";
            List<DbParameter> paramenters = new List<DbParameter>();
            paramenters.Add(dbHelper.NewDbParameter("@CID", DbType.String, CID, 36));
            DataTable dt = dbHelper.GetDataTable(sqlStr, paramenters.ToArray(), null);
            dt.TableName = "DBTable";
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            DBManager.DBTable retObj = dt.Rows[0].DataRowToModel<DBManager.DBTable>(true, false);
            //DBTableCol
            sqlStr = "select * from DBTableCol where CDBTableID=@CDBTableID ";
            paramenters = new List<DbParameter>();
            paramenters.Add(dbHelper.NewDbParameter("@CDBTableID", DbType.String, retObj.CID, 36));
            dt = dbHelper.GetDataTable(sqlStr, paramenters.ToArray(), null);
            dt.TableName = "DBTableCol";
            retObj.CCols = dt.DataTableToList<DBManager.DBTableCol>(true, false);
            return retObj;
        }

        public List<DBManager.DBTable> GetAllTable()
        {
            string sqlStr = " select * from DBTable ";
            DataTable dt = dbHelper.GetDataTable(sqlStr, null, null);
            dt.TableName = "DBTable";
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            List<DBManager.DBTable> retObj = dt.DataTableToList<DBManager.DBTable>(true, false);
            foreach (var t in retObj)
            {
                //DBTableCol
                sqlStr = "select * from DBTableCol where CDBTableID=@CDBTableID ";
                List<DbParameter> paramenters = new List<DbParameter>();
                paramenters.Add(dbHelper.NewDbParameter("@CDBTableID", DbType.String, t.CID, 36));
                dt = dbHelper.GetDataTable(sqlStr, paramenters.ToArray(), null);
                dt.TableName = "DBTableCol";
                t.CCols = dt.DataTableToList<DBManager.DBTableCol>(true, false);
            }
            return retObj;
        }

        public void AddTable(DBManager.DBTable dbTable)
        {
            using (DBHelper.DBHelperBase.DBTransactionHelper transaction = new DBHelper.DBHelperBase.DBTransactionHelper(dbHelper))
            {
                string sqlStr = @"create table [{0}]({1}
                                 CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
                                ({2})WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                ) ON [PRIMARY] {3} {4}";
                string sqlCol = "";//列
                string sqlPKey = "";//主键
                string sqlIndex = "";//索引
                string sqlDefaultValue = "";//默认值
                foreach (var col in dbTable.CCols)
                {
                    string sizeStr = "";
                    if (col.CSize != null && col.CDecimalsSize != null)
                    {
                        sizeStr = String.Format("({0}, {1})", col.CSize, col.CDecimalsSize);
                    }
                    else if (col.CSize != null)
                    {
                        sizeStr = String.Format("({0})", col.CSize);
                    }
                    sqlCol += String.Format("[{0}] {1}{2} {3},", col.CID, GetColTypeStr(col.CType), sizeStr, col.CNotNULL ? "NOT NULL" : "NULL");
                    if (col.CPK)//主键
                    {
                        sqlPKey += "["+col.CID + "] ASC,";
                    }
                    if (col.CIX == DBTableCol.IXType.普通索引)
                    {
                        sqlIndex += String.Format(" CREATE NONCLUSTERED INDEX [IX_{0}_{1}] ON dbo.[{0}] ([{1}]) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ", dbTable.CID, col.CID);
                    }
                    else if (col.CIX == DBTableCol.IXType.唯一索引)
                    {
                        sqlIndex += String.Format(" CREATE UNIQUE NONCLUSTERED INDEX [IX_{0}_{1}] ON dbo.[{0}] ([{1}]) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ", dbTable.CID, col.CID);
                    }
                    if (col.CDefaultValue != null)
                    {
                        sqlDefaultValue += String.Format(" ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_{1}]  DEFAULT ({2}) FOR [{1}] ", dbTable.CID, col.CID, (col.CType == DBTableCol.ColType.文本 || col.CType == DBTableCol.ColType.英文) ? "'" + col.CDefaultValue + "'" : col.CDefaultValue);
                    }
                }
                sqlCol = sqlCol.Substring(0, sqlCol.Length - 1);
                sqlPKey = sqlPKey.Substring(0, sqlPKey.Length - 1);
                dbTable.CCTime = dbHelper.GetDBDateTime();
                dbTable.CUTime = dbTable.CCTime;
                foreach (var col in dbTable.CCols)
                {
                    col.CCTime = dbHelper.GetDBDateTime();
                    col.CUTime = col.CCTime;
                }
                dbHelper.ExecuteNonQuery(String.Format(sqlStr, dbTable.CID, sqlCol, sqlPKey, sqlIndex, sqlDefaultValue), null, null);
                dbHelper.AddDataTable(dbTable.ModelToDataTableHasValue(true, true), null);
                dbHelper.AddDataTable(dbTable.CCols.ListToDataTable<DBManager.DBTableCol>(true, true), null);
                transaction.Commit();
            }
        }

        public void UpdateTable(DBManager.DBTable dbTable)
        {
            throw new NotImplementedException();
        }

        public void DeleteTable(string CID)
        {
            throw new NotImplementedException();
        }
        private string GetColTypeStr(DBTableCol.ColType colType)
        {
            string colTypeStr = null;
            switch (colType)
            {
                case DBTableCol.ColType.文本:
                    colTypeStr = "nvarchar";
                    break;
                case DBTableCol.ColType.数字:
                    colTypeStr = "decimal";
                    break;
                case DBTableCol.ColType.是否:
                    colTypeStr = "bit";
                    break;
                case DBTableCol.ColType.选项:
                    colTypeStr = "nvarchar";
                    break;
                case DBTableCol.ColType.英文:
                    colTypeStr = "varchar";
                    break;
            }
            return colTypeStr;
        }
        #endregion
    }
}
