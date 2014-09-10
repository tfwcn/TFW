using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace DAL
{
    /// <summary>
    /// 数据库操作基类(for PostgreSQL)
    /// </summary>
    public class PostgreHelper
    {
        private DbConnection GetConnection()
        {
            string connectionString = "Server=localhost;Port=5432;User Id=postgres;"
                + "Password=SA123; Database=databaseName;"
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
                if (conn != null)
                    conn.Close();
            }
        }
    }
}
