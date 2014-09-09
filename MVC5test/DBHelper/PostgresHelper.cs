using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Npgsql;

namespace DBHelper
{
    public class PostgresHelper
    {
        private DbConnection GetConnection()
        {
            DbConnection conn = null;
            return conn;
        }
        public DbDataReader GetDataReader()
        {
            NpgsqlConnection conn = GetConnection() as NpgsqlConnection;
            conn.Open();
            DbCommand com = new NpgsqlCommand("",conn);
            DbDataReader dr = com.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }
    }
}
