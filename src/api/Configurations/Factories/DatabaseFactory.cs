using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using API.Domains.Models.Options;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace API.Configurations.Factories
{
    public interface IDatabaseFactory
    {
        Task<IDbTransaction> BeginTransactionAsync();
        IDbConnection Connection();
        Task OpenConnectionAsync();
        void CommitTransaction();
        void RollbackTransaction();
        void CloseConnection();
    }

    public class DatabaseFactory : IDatabaseFactory
    {
        //private readonly MySqlConnection _connection;
        private readonly SqlConnection _connection;
        //private MySqlTransaction _transaction;
        private SqlTransaction _transaction;
        private bool _isTransactionOpen;

        public DatabaseFactory(IOptions<Domains.Models.Options.Database> database)
        {
            //var connectionstring = $"Server={database.Value.Server};Port={database.Value.Port};Database={database.Value.Schema};Uid={database.Value.User};Pwd={database.Value.Password};";
            var connectionstring = $"Data Source={database.Value.Server};Initial Catalog={database.Value.Schema};User Id={database.Value.User};Password={database.Value.Password};";

            //_connection = new MySqlConnection(connectionstring);
            _connection = new SqlConnection(connectionstring);
        }

        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                if (_connection.State != ConnectionState.Open)
                    throw new Exception("A conexão com o banco não esta aberta.");

                _transaction =  _connection.BeginTransaction();
            }

            _isTransactionOpen = true;

            return _transaction;
        }

        public IDbConnection Connection()
        {
            return _connection;
        }

        public async Task OpenConnectionAsync()
        {
            //var connection = _connection as MySqlConnection;
            var connection = _connection as SqlConnection;

            await connection.OpenAsync();
        }

        public void CommitTransaction()
        {
            if (_isTransactionOpen)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }

            _isTransactionOpen = false;
        }

        public void RollbackTransaction()
        {
            if (_isTransactionOpen)
                _transaction.Rollback();
        }

        public void CloseConnection()
        {
            _connection.Close();

            _connection.Dispose();
        }
    }

}