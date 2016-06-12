using System;
using System.Collections.Generic;
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
    public class Login
    {
        public int Id { get; set; }

        public string UserName;

        public bool IsAuthorized;

        public static string TableName = "Login";

        public static int Update(Login value)
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(Statics.GetConnectionString());
                connection.Open();
                SqliteCommand command = connection.CreateCommand();

                string commandStr = string.Format("UPDATE {0} SET UserName='{1}',IsAuthorized='{2}' WHERE Id='{3}'", TableName,
                    value.UserName, value.IsAuthorized, value.Id);

                command.CommandText = commandStr;

                int result = command.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return -1;
#endif

            }
            
        }

        public static int Insert(Login value)
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(Statics.GetConnectionString());
                connection.Open();
                SqliteCommand command = connection.CreateCommand();

                string commandStr = string.Format("INSERT INTO Login (UserName,IsAuthorized) VALUES('{0}','{1}');SELECT last_insert_rowid();", value.UserName, value.IsAuthorized);
                command.CommandText = commandStr;

                var result = command.ExecuteScalar();

                connection.Close();

                int id = 0;

                if (result != null)
                {
                    id = Convert.ToInt32(result);

                    return id;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return -1;
#endif
            }

            return -1;
        }

        public static List<Login> GetValues()
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(Statics.GetConnectionString());
                connection.Open();
                SqliteCommand command = connection.CreateCommand();

                string commandStr = "SELECT * FROM Login;";
                command.CommandText = commandStr;

                SqliteDataReader reader = command.ExecuteReader();

                List<Login> result = new List<Login>();

                while (reader.Read())
                {
                    Login login = new Login();
                    login.Id = Convert.ToInt32(reader["Id"]);
                    login.UserName = reader["UserName"] as string;
                    login.IsAuthorized = Convert.ToBoolean(reader["IsAuthorized"]);

                    result.Add(login);
                }

                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return null;
#endif
            }
        }

        public static List<Login> GetValues(Func<Login,bool> predicate)
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(Statics.GetConnectionString());
                connection.Open();
                SqliteCommand command = connection.CreateCommand();

                string commandStr = "SELECT * FROM Login;";
                command.CommandText = commandStr;

                SqliteDataReader reader = command.ExecuteReader();

                List<Login> result = new List<Login>();

                while (reader.Read())
                {
                    Login login = new Login();
                    login.Id = Convert.ToInt32(reader["Id"]);
                    login.UserName =reader["UserName"] as string;
                    login.IsAuthorized =Convert.ToBoolean(reader["IsAuthorized"]);

                    if (predicate(login))
                    {
                        result.Add(login);
                    }
                }
                
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return null;
#endif
            }

        }
    }
}