using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;

namespace TechnicalFanCoilAndroid.Model
{
    public class Table
    {
        public SqliteDataAdapter DataAdapter { set; get; }
        public DataTable DataTable { get; set; }
        public SqliteCommandBuilder CommandBuilder { get; set; }
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        private int _updateBatchSize = 1;

        private SqliteConnection _connection;
        private SqliteTransaction _transaction;
        private bool _isInTransaction = false;

        public SqliteConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public SqliteTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        public bool IsInTransaction
        {
            get
            {
                if (this.Connection == null || this.Transaction == null)
                {
                    _isInTransaction = false;
                }
                else
                {
                    _isInTransaction = true;
                }

                return _isInTransaction;
            }
            set { _isInTransaction = value; }
        }

        //public int UpdateBatchSize
        //{
        //    get { return _updateBatchSize; }
        //    set
        //    {
        //        _updateBatchSize = value;
        //        DataAdapter.UpdateBatchSize = _updateBatchSize;
        //    }
        //}

        public Table(string connectionString, string tableName)
        {
            this.ConnectionString = connectionString;
            SqliteConnection connection = new SqliteConnection(this.ConnectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            this.TableName = tableName;
            command.CommandText = string.Format(@"SELECT * FROM {0}", tableName);


            DataAdapter = new SqliteDataAdapter(command);
            CommandBuilder = new SqliteCommandBuilder(DataAdapter);
            DataTable = new DataTable(tableName);
            DataAdapter.Fill(DataTable);
            connection.Close();
        }

        public Table(string connectionString, string tableName, SqliteCommand command)
        {
            this.ConnectionString = connectionString;
            SqliteConnection connection = new SqliteConnection(this.ConnectionString);
            connection.Open();
            command.Connection = connection;
            this.TableName = tableName;
            DataAdapter = new SqliteDataAdapter(command);
            CommandBuilder = new SqliteCommandBuilder(DataAdapter);
            DataTable = new DataTable(tableName);
            DataAdapter.Fill(DataTable);
            connection.Close();
        }

        public Table(string connectionString, SqliteTransaction transaction, string tableName)
        {
            this.ConnectionString = connectionString;
            SqliteConnection connection = transaction.Connection;
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandText = string.Format(@"SELECT * FROM {0}", tableName);
            this.TableName = tableName;
            DataAdapter = new SqliteDataAdapter(command);
            CommandBuilder = new SqliteCommandBuilder(DataAdapter);
            DataTable = new DataTable(tableName);
            DataAdapter.Fill(DataTable);
        }

        public Table(string connectionString, SqliteTransaction transaction, string tableName, SqliteCommand command)
        {
            this.ConnectionString = connectionString;
            SqliteConnection connection = transaction.Connection;
            command.Connection = connection;
            command.Transaction = transaction;
            this.TableName = tableName;
            DataAdapter = new SqliteDataAdapter(command);
            CommandBuilder = new SqliteCommandBuilder(DataAdapter);
            DataTable = new DataTable(tableName);
            DataAdapter.Fill(DataTable);
        }

        public static void CreateDatabase(string path)
        {
            SqliteConnection.CreateFile(path);
        }

        public static void CreateTable(string connectionString, SqliteCommand command)
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            command.Connection = connection;
            command.ExecuteNonQuery();
            connection.Clone();
        }

        public static void CreateTable(SqliteTransaction transaction, SqliteCommand command)
        {
            SqliteConnection connection = transaction.Connection;
            connection.Open();
            command.Connection = connection;
            command.Transaction = transaction;
            command.ExecuteNonQuery();
        }

        public static bool TableExist(string connectionString, string tableName)
        {
            // TODO
            return false;
        }

        public static bool TableExist(SqliteConnection transaction, string tableName)
        {
            // TODO
            return false;
        }

        public static void DropTable(string connectionString, string tableName)
        {
            SqliteConnection connection = null;
            SqliteCommand command = new SqliteCommand();

            connection = new SqliteConnection(connectionString);
            connection.Open();
            command.Connection = connection;

            string commandText5 = string.Format(
@"DROP Table {0};", tableName);

            command.CommandText = commandText5;
            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void DropTable(SqliteTransaction transaction, string tableName)
        {
            SqliteConnection connection = transaction.Connection;
            SqliteCommand command = new SqliteCommand();

            command.Connection = connection;
            command.Transaction = transaction;

            string commandText5 = string.Format(
@"DROP Table {0};", tableName);

            command.CommandText = commandText5;
            command.ExecuteNonQuery();
        }
    }
}