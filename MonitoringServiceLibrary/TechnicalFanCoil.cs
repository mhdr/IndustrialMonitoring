using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using NationalInstruments.NetworkVariable;
using SharedLibrary;

namespace MonitoringServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TechnicalFanCoil" in both code and config file together.
    public class TechnicalFanCoil : ITechnicalFanCoil
    {
        public void DoWork()
        {
        }

        public bool TurnOffMotor1()
        {
            try
            {
                string location = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor1Off_M";
                var writer = new NetworkVariableBufferedWriter<bool>(location);
                writer.Connect();
                writer.WriteValue(true);
                writer.Disconnect();

                Thread.Sleep(2 * 1000);

                writer =new NetworkVariableBufferedWriter<bool>(location);
                writer.Connect();
                writer.WriteValue(false);
                writer.Disconnect();

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }

            return false;
        }

        public bool TurnOffMotor2()
        {
            try
            {
                string location = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor2Off_M";
                var writer = new NetworkVariableBufferedWriter<bool>(location);
                writer.Connect();
                writer.WriteValue(true);
                writer.Disconnect();

                Thread.Sleep(2 * 1000);

                writer=new NetworkVariableBufferedWriter<bool>(location);
                writer.Connect();
                writer.WriteValue(false);
                writer.Disconnect();

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }

            return false;
        }

        public bool ChangeSpeedMotor1(int speed)
        {
            try
            {
                TurnOffMotor1();

                string speed1 = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor1Speed1_M";
                string speed2 = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor1Speed2_M";
                string speed3 = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor1Speed3_M";

                if (speed > 0)
                {
                    Thread.Sleep(5 * 1000);
                }

                if (speed == 1)
                {
                    var writer1 = new NetworkVariableBufferedWriter<bool>(speed1);
                    writer1.Connect();
                    writer1.WriteValue(true);
                    writer1.Disconnect();

                    Thread.Sleep(2 * 1000);

                    writer1 = new NetworkVariableBufferedWriter<bool>(speed1);
                    writer1.Connect();
                    writer1.WriteValue(false);
                    writer1.Disconnect();
                }
                else if (speed == 2)
                {
                    var writer2 = new NetworkVariableBufferedWriter<bool>(speed2);
                    writer2.Connect();
                    writer2.WriteValue(true);
                    writer2.Disconnect();

                    Thread.Sleep(2 * 1000);

                    writer2 = new NetworkVariableBufferedWriter<bool>(speed2);
                    writer2.Connect();
                    writer2.WriteValue(false);
                    writer2.Disconnect();
                }
                else if (speed == 3)
                {
                    var writer3 = new NetworkVariableBufferedWriter<bool>(speed3);
                    writer3.Connect();
                    writer3.WriteValue(true);
                    writer3.Disconnect();

                    Thread.Sleep(2 * 1000);

                    writer3 = new NetworkVariableBufferedWriter<bool>(speed3);
                    writer3.Connect();
                    writer3.WriteValue(false);
                    writer3.Disconnect();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }

            return false;
        }

        public bool ChangeSpeedMotor2(int speed)
        {
            try
            {
                TurnOffMotor2();

                string speed1 = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor2Speed1_M";
                string speed2 = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor2Speed2_M";
                string speed3 = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor2Speed3_M";

                if (speed > 0)
                {
                    Thread.Sleep(5 * 1000);
                }

                if (speed == 1)
                {
                    var writer1 = new NetworkVariableBufferedWriter<bool>(speed1);
                    writer1.Connect();
                    writer1.WriteValue(true);
                    writer1.Disconnect();

                    Thread.Sleep(2 * 1000);

                    writer1 = new NetworkVariableBufferedWriter<bool>(speed1);
                    writer1.Connect();
                    writer1.WriteValue(false);
                    writer1.Disconnect();
                }
                else if (speed == 2)
                {
                    var writer2 = new NetworkVariableBufferedWriter<bool>(speed2);
                    writer2.Connect();
                    writer2.WriteValue(true);
                    writer2.Disconnect();

                    Thread.Sleep(2 * 1000);

                    writer2 = new NetworkVariableBufferedWriter<bool>(speed2);
                    writer2.Connect();
                    writer2.WriteValue(false);
                    writer2.Disconnect();
                }
                else if (speed == 3)
                {
                    var writer3 = new NetworkVariableBufferedWriter<bool>(speed3);
                    writer3.Connect();
                    writer3.WriteValue(true);
                    writer3.Disconnect();

                    Thread.Sleep(2 * 1000);

                    writer3 = new NetworkVariableBufferedWriter<bool>(speed3);
                    writer3.Connect();
                    writer3.WriteValue(false);
                    writer3.Disconnect();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }

            return false;
        }
    }
}
