using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FillingServiceLibrary
{
    public class Table
    {
        public SqlDataAdapter DataAdapter { set; get; }
        public DataTable DataTable { get; set; }
        public SqlCommandBuilder CommandBuilder { get; set; }
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        private int _updateBatchSize = 1;

        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private bool _isInTransaction = false;

        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public SqlTransaction Transaction
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

        public int UpdateBatchSize
        {
            get { return _updateBatchSize; }
            set
            {
                _updateBatchSize = value;
                DataAdapter.UpdateBatchSize = _updateBatchSize;
            }
        }

        public Table(string connectionString, string tableName)
        {
            this.ConnectionString = connectionString;
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            this.TableName = tableName;
            command.CommandText = string.Format(@"SELECT * FROM {0}", tableName);


            DataAdapter = new SqlDataAdapter(command);
            CommandBuilder = new SqlCommandBuilder(DataAdapter);
            DataTable = new DataTable(tableName);
            DataAdapter.Fill(DataTable);
            connection.Close();
        }

        public Table(string connectionString, string tableName, SqlCommand command)
        {
            this.ConnectionString = connectionString;
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            connection.Open();
            command.Connection = connection;
            this.TableName = tableName;
            DataAdapter = new SqlDataAdapter(command);
            CommandBuilder = new SqlCommandBuilder(DataAdapter);
            DataTable = new DataTable(tableName);
            DataAdapter.Fill(DataTable);
            connection.Close();
        }

        public Table(string connectionString, SqlTransaction transaction, string tableName)
        {
            this.ConnectionString = connectionString;
            SqlConnection connection = transaction.Connection;
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandText = string.Format(@"SELECT * FROM {0}", tableName);
            this.TableName = tableName;
            DataAdapter = new SqlDataAdapter(command);
            CommandBuilder = new SqlCommandBuilder(DataAdapter);
            DataTable = new DataTable(tableName);
            DataAdapter.Fill(DataTable);
        }

        public Table(string connectionString, SqlTransaction transaction, string tableName, SqlCommand command)
        {
            this.ConnectionString = connectionString;
            SqlConnection connection = transaction.Connection;
            command.Connection = connection;
            command.Transaction = transaction;
            this.TableName = tableName;
            DataAdapter = new SqlDataAdapter(command);
            CommandBuilder = new SqlCommandBuilder(DataAdapter);
            DataTable = new DataTable(tableName);
            DataAdapter.Fill(DataTable);
        }

        public void DropTable()
        {
            if (IsInTransaction)
            {
                SqlConnection connection = Transaction.Connection;
                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.Transaction = Transaction;

                string commandText5 = string.Format(
    @"DROP Table {0};", TableName);

                command.CommandText = commandText5;
                command.ExecuteNonQuery();
            }
            else
            {
                SqlConnection connection = null;
                SqlCommand command = new SqlCommand();

                connection = new SqlConnection(this.ConnectionString);
                connection.Open();
                command.Connection = connection;

                string commandText5 = string.Format(
    @"DROP Table {0};", TableName);

                command.CommandText = commandText5;
                command.ExecuteNonQuery();

                connection.Close();
            }

        }
    }
}
