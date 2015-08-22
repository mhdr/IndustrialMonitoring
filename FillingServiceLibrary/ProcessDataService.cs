using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FillingServiceLibrary
{
    public class ProcessDataService : IProcessDataService
    {
        public double GetPreHeatingZoneTemperature()
        {
            SqlConnection connection=new SqlConnection(Statics.WinCC_ConnectionString);
            string cmd = @"SELECT TOP 1 * FROM Logs WHERE Logs.ItemId=1 ORDER BY LogId DESC;";
            SqlCommand command = new SqlCommand(cmd,connection);

            SqlDataReader reader = command.ExecuteReader();

            double value = (double) reader["ItemValue"];

            connection.Close();

            return value;
        }

        public double GetSterilizerZoneTemperature()
        {
            SqlConnection connection = new SqlConnection(Statics.WinCC_ConnectionString);
            string cmd = @"SELECT TOP 1 * FROM Logs WHERE Logs.ItemId=2 ORDER BY LogId DESC;";
            SqlCommand command = new SqlCommand(cmd, connection);

            SqlDataReader reader = command.ExecuteReader();

            double value = (double)reader["ItemValue"];

            connection.Close();

            return value;
        }

        public double GetCoolingZoneTemperature()
        {
            SqlConnection connection = new SqlConnection(Statics.WinCC_ConnectionString);
            string cmd = @"SELECT TOP 1 * FROM Logs WHERE Logs.ItemId=3 ORDER BY LogId DESC;";
            SqlCommand command = new SqlCommand(cmd, connection);

            SqlDataReader reader = command.ExecuteReader();

            double value = (double)reader["ItemValue"];

            connection.Close();

            return value;
        }
    }
}
