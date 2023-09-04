using System;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseHelper
{
   
    public class Database : IDisposable
    {
        private bool _useSingleConnection;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public Database(string connectionString, bool useSingleConnection = false)
        {
            ConnectionString = connectionString;
            _useSingleConnection = useSingleConnection;
        }

        public string ConnectionString { get; private init; }

        public SqlConnection GetConnction()
        {
            if (_connection == default || !_useSingleConnection)
            {
                _connection = new SqlConnection(ConnectionString);
            }
            return _connection;
        }

        public void BeginTransaction()
        {
            if (!_useSingleConnection)
            {
                throw new Exception("Transaction is only supported on single connection mode");
            }
            if (_transaction != default)
            {
                throw new Exception("Transaction is already active");
            }

            _connection = GetConnction();
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction == default)
            {
                throw new Exception("Transaction is no active transaction");
            }
            _transaction.Commit();
            _transaction = default;
        }

        public void RollbackTransaction()
        {
            if (_transaction == default)
            {
                throw new Exception("Transaction is no active transaction");
            }
            _transaction.Rollback();
            _transaction = default;
        }

        public SqlCommand GetCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = new SqlCommand
            {
                Connection = GetConnction(),
                CommandText = commandText,
                CommandType = commandType
            };
            command.Parameters.AddRange(parameters);
            if (_transaction != default)
            {
                command.Transaction = _transaction;
            }
            return command;
        }

        public SqlCommand GetCommand(string commandText, params SqlParameter[] parameters)
        {
            return GetCommand(commandText, CommandType.Text, parameters);
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = GetCommand(commandText, commandType, parameters);
            try
            {
                if (_connection.State != ConnectionState.Open) command.Connection.Open();
                
                return command.ExecuteNonQuery();
            }
            finally
            {
                if (_transaction == default)command.Connection.Close();
            }
        }

        public int ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(commandText, CommandType.Text, parameters);
        }

        public object ExecuteScalar(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = GetCommand(commandText, commandType, parameters);
            try
            {
                command.Connection.Open();
                return command.ExecuteScalar();
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public object ExecuteScalar(string commandText, params SqlParameter[] parameters)
        {
            return ExecuteScalar(commandText, CommandType.Text, parameters);
        }

        public SqlDataReader ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = GetCommand(commandText, commandType, parameters);
            command.Connection.Open();
            var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        public DataTable GetTable(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = GetCommand(commandText, commandType, parameters);
            try
            {
                if (_connection.State != ConnectionState.Open) command.Connection.Open();
                var table = new DataTable();
                table.Load(command.ExecuteReader());
                return table;
            }
            finally
            {
                if (_transaction == default)command.Connection.Close();
            }
        }

        public DataTable GetTable(string commandText, params SqlParameter[] parameters)
        {
            return GetTable(commandText, CommandType.Text, parameters);
        }

        public void Dispose()
        {
            if (_connection != default && _useSingleConnection)
            {
                _connection.Close();
            }
            if (_transaction != default)
            {
                CommitTransaction();
            }
        }
    }
}
