using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.NetworkVariable;

namespace MonitoringServiceLibrary
{
    public class OpcCommandsJob
    {
        public void Run()
        {
            IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

            var listCommands = entities.OpcBooleanCommands.ToList();

            foreach (OpcBooleanCommand command in listCommands)
            {
                var location = command.Location;
                var value = command.CommandValue;

                if (command.PreviousRun == null)
                {
                    var writer = new NetworkVariableBufferedWriter<bool>(location);
                    writer.Connect();
                    writer.WriteValue(value);
                    writer.Disconnect();

                    command.PreviousRun=DateTime.Now;
                    entities.SaveChanges();
                }
                else
                {
                    var datetime2 = DateTime.Now;
                    var datetime1 = command.PreviousRun;
                    var interval = command.Interval;

                    var diff = datetime2 - datetime1;

                    if (diff.Value.TotalSeconds > interval)
                    {
                        var writer = new NetworkVariableBufferedWriter<bool>(location);
                        writer.Connect();
                        writer.WriteValue(value);
                        writer.Disconnect();

                        command.PreviousRun = DateTime.Now;
                        entities.SaveChanges();
                    }
                }
            }
        }
    }
}
