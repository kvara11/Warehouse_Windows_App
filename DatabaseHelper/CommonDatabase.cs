using System.Data;
using System.Data.SqlClient;

namespace DatabaseHelper
{
    public class CommonDatabase<TConnection> where TConnection : class, IDbConnection, new()
    {
        private bool _useSingleConnection;
        private TConnection _connection;

        public CommonDatabase(string connectionString, bool useSingleConnection = false)
        {
            ConnectionString = connectionString;
            _useSingleConnection = useSingleConnection;
        }

        public string ConnectionString { get; private init; }

        public TConnection GetConnction()
        {
            if (_connection == default || !_useSingleConnection)
            {
                _connection = new TConnection();
                _connection.ConnectionString = ConnectionString;
            }
            return _connection;
        }
    }

    public class SqlDatabase : CommonDatabase<SqlConnection>
    {
        public SqlDatabase(string connectionString, bool useSingleConnection = false) : base(connectionString, useSingleConnection)
        {

        }



    }
}