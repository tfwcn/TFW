using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Npgsql;

namespace DBHelper
{
    /// <summary>
    /// 数据库操作基类(for PostgreSQL)
    /// </summary>
    public class PostgreHelper
    {
        private DbConnection GetConnection()
        {
            string connectionString = "Server=localhost;Port=5432;User Id=postgres;"
                + "Password=sa123; Database=TESTDB;"
                + "CommandTimeout=0;ConnectionLifeTime=0;";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            return conn;
        }
        private DbCommand GetCommand(string cmdText, DbConnection connection)
        {
            NpgsqlConnection conn = connection as NpgsqlConnection;
            NpgsqlCommand com = new NpgsqlCommand(cmdText, conn);
            return com;
        }
        private DbCommand GetCommand(string cmdText, DbConnection connection, NpgsqlTransaction transaction)
        {
            NpgsqlConnection conn = connection as NpgsqlConnection;
            NpgsqlCommand com = new NpgsqlCommand(cmdText, conn, transaction);
            return com;
        }
        public DbDataReader GetDataReader(string cmdText, DbParameter[] paramenters)
        {
            DbConnection conn = GetConnection();
            DbCommand com = GetCommand(cmdText, conn);
            try
            {
                conn.Open();
                return com.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                //if (conn != null)
                //    conn.Close();
            }
        }
        public int RunSQL(string cmdText, DbParameter[] paramenters)
        {
            DbConnection conn = GetConnection();
            DbCommand com = GetCommand(cmdText, conn);
            try
            {
                conn.Open();
                return com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
        public object GetObject(string cmdText, DbParameter[] paramenters)
        {
            DbConnection conn = GetConnection();
            DbCommand com = GetCommand(cmdText, conn);
            try
            {
                conn.Open();
                return com.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
    }
}
