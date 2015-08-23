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
            double value = 0;
            try
            {
                SqlConnection connection = new SqlConnection(Statics.WinCC_ConnectionString);
                connection.Open();
                string cmd = @"SELECT TOP 1 * FROM Logs WHERE Logs.ItemId=1 ORDER BY LogId DESC;";
                SqlCommand command = new SqlCommand(cmd, connection);

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                value = (double)reader["ItemValue"];

                connection.Close();

                string output = string.Format("Pre Heating Zone Temperature : {0} , Time : {1}", value, DateTime.Now);
                Console.WriteLine(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return value;
        }

        public double GetSterilizerZoneTemperature()
        {
            double value = 0;
            try
            {
                SqlConnection connection = new SqlConnection(Statics.WinCC_ConnectionString);
                connection.Open();
                string cmd = @"SELECT TOP 1 * FROM Logs WHERE Logs.ItemId=2 ORDER BY LogId DESC;";
                SqlCommand command = new SqlCommand(cmd, connection);

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                value = (double)reader["ItemValue"];

                connection.Close();

                string output = string.Format("Sterilizer Zone Temperature : {0} , Time : {1}", value, DateTime.Now);
                Console.WriteLine(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return value;
        }

        public double GetCoolingZoneTemperature()
        {
            double value = 0;
            try
            {
                SqlConnection connection = new SqlConnection(Statics.WinCC_ConnectionString);
                connection.Open();
                string cmd = @"SELECT TOP 1 * FROM Logs WHERE Logs.ItemId=3 ORDER BY LogId DESC;";
                SqlCommand command = new SqlCommand(cmd, connection);

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                value = (double)reader["ItemValue"];

                connection.Close();

                string output = string.Format("Cooling Zone Temperature : {0} , Time : {1}", value, DateTime.Now);
                Console.WriteLine(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return value;
        }
    }
}
