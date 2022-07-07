using System;
using System.Data;
using System.Data.SqlClient;

namespace Libraries
{
    public class ShopDBContext
    {
        private readonly string _connectionString;

        public ShopDBContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
