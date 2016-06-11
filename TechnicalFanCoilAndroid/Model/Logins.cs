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
using TechnicalFanCoilAndroid.Model;

namespace TechnicalFanCoilAndroid.Model
{
    public class Logins
    {
        private string tableName = "Logins";
        public SqliteConnection Connection { get; private set; }
        public SqliteTransaction Transaction { get; private set; }
        private bool _isInTransaction = false;

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

        public string TableName
        {
            get { return tableName; }
        }

        public static void CreateTable(string connectionString)
        {
            string tableName = "People";
            string command = string.Format(@"CREATE TABLE IF NOT EXISTS '{0}' (
'Id'  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
'UserName'  TEXT NOT NULL,
'IsAuthorized'  TEXT NOT NULL
);", tableName);

            Table.CreateTable(connectionString, new SqliteCommand(command));
        }

        public List<Login> GetValue()
        {
            Table table = new Table(Statics.GetConnectionString(), this.tableName);

            List<Login> result = new List<Login>();

            foreach (DataRow row in table.DataTable.Rows)
            {
                Login item = new Login();
                item.Id = (int)row["Id"];
                item.UserName = row["UserName"] as string;
                item.IsAuthorized = (bool) row["IsAuthorized"];
                result.Add(item);
            }

            return result;
        }

        public List<Login> GetValue(Func<Login, bool> predicate)
        {
            Table table = new Table(Statics.GetConnectionString(), this.tableName);

            List<Login> result = new List<Login>();

            foreach (DataRow row in table.DataTable.Rows)
            {
                Login item = new Login();
                item.Id = (int)row["Id"];
                item.UserName = row["UserName"] as string;
                item.IsAuthorized = (bool)row["IsAuthorized"];
                result.Add(item);

                if (predicate(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public void Insert(Login value)
        {
            if (string.IsNullOrEmpty(this.TableName))
            {
                throw new NullReferenceException("TableName is Null");
            }

            Table table = null;
            if (this.IsInTransaction)
            {
                table = new Table(Statics.GetConnectionString(), this.Transaction, this.TableName);
            }
            else
            {
                table = new Table(Statics.GetConnectionString(), this.TableName);
            }

            SqliteCommand insertCommand = table.CommandBuilder.GetInsertCommand().Clone() as SqliteCommand;
            insertCommand.CommandText += ";SET @LastId= SCOPE_IDENTITY();";

            SqliteParameter param = new SqliteParameter();
            param.Direction = ParameterDirection.Output;
            param.DbType = DbType.Int32;
            param.ParameterName = "@LastId";

            insertCommand.Parameters.Add(param);
            table.DataAdapter.InsertCommand = insertCommand;

            DataRow newRow = table.DataTable.NewRow();
            newRow["Id"] = value.Id;
            newRow["UserName"] = value.UserName;
            newRow["IsAuthorized"] = value.IsAuthorized;
            table.DataTable.Rows.Add(newRow);
            table.DataAdapter.Update(table.DataTable);

            value.Id = (int)param.Value;
        }
    }
}