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

                Thread.Sleep(5 * 1000);

                writer =new NetworkVariableBufferedWriter<bool>(location);
                writer.Connect();
                writer.WriteValue(false);
                writer.Disconnect();

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary2(ex);
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

                Thread.Sleep(5 * 1000);

                writer=new NetworkVariableBufferedWriter<bool>(location);
                writer.Connect();
                writer.WriteValue(false);
                writer.Disconnect();

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary2(ex);
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

                    Thread.Sleep(5 * 1000);

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

                    Thread.Sleep(5 * 1000);

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

                    Thread.Sleep(5 * 1000);

                    writer3 = new NetworkVariableBufferedWriter<bool>(speed3);
                    writer3.Connect();
                    writer3.WriteValue(false);
                    writer3.Disconnect();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary2(ex);
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

                    Thread.Sleep(5 * 1000);

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

                    Thread.Sleep(5 * 1000);

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

                    Thread.Sleep(5 * 1000);

                    writer3 = new NetworkVariableBufferedWriter<bool>(speed3);
                    writer3.Connect();
                    writer3.WriteValue(false);
                    writer3.Disconnect();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary2(ex);
            }

            return false;
        }

        public string GetStatus()
        {
            try
            {
                string motor1Speed1Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor1Speed1_Q";
                string motor1Speed2Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor1Speed2_Q";
                string motor1Speed3Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor1Speed3_Q";
                string motor2Speed1Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor2Speed1_Q";
                string motor2Speed2Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor2Speed2_Q";
                string motor2Speed3Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor2Speed3_Q";

                bool motor1Speed1;
                bool motor1Speed2;
                bool motor1Speed3;
                bool motor2Speed1;
                bool motor2Speed2;
                bool motor2Speed3;
                
                var subscriberBool1 = new NetworkVariableBufferedSubscriber<bool>(motor1Speed1Path);
                subscriberBool1.Connect();
                var variable1 = subscriberBool1.ReadData();
                motor1Speed1 = variable1.GetValue();
                subscriberBool1.Disconnect();

                var subscriberBool2 = new NetworkVariableBufferedSubscriber<bool>(motor1Speed2Path);
                subscriberBool2.Connect();
                var variable2 = subscriberBool2.ReadData();
                motor1Speed2 = variable2.GetValue();
                subscriberBool2.Disconnect();

                var subscriberBool3 = new NetworkVariableBufferedSubscriber<bool>(motor1Speed3Path);
                subscriberBool3.Connect();
                var variable3 = subscriberBool3.ReadData();
                motor1Speed3 = variable3.GetValue();
                subscriberBool3.Disconnect();

                var subscriberBool4 = new NetworkVariableBufferedSubscriber<bool>(motor2Speed1Path);
                subscriberBool4.Connect();
                var variable4 = subscriberBool4.ReadData();
                motor2Speed1 = variable4.GetValue();
                subscriberBool4.Disconnect();

                var subscriberBool5 = new NetworkVariableBufferedSubscriber<bool>(motor2Speed2Path);
                subscriberBool5.Connect();
                var variable5 = subscriberBool5.ReadData();
                motor2Speed2 = variable5.GetValue();
                subscriberBool5.Disconnect();

                var subscriberBool6 = new NetworkVariableBufferedSubscriber<bool>(motor2Speed3Path);
                subscriberBool6.Connect();
                var variable6 = subscriberBool6.ReadData();
                motor2Speed3 = variable6.GetValue();
                subscriberBool6.Disconnect();

                string result1 = "";
                string result2 = "";

                if (motor1Speed1 == false && motor1Speed2 == false && motor1Speed3 == false)
                {
                    result1 = "Motor 1 is Off";
                }
                else if (motor1Speed1)
                {
                    result1 = "Motor 1 is On and Speed is 1";
                }
                else if (motor1Speed2)
                {
                    result1 = "Motor 1 is On and Speed is 2";
                }
                else if (motor1Speed3)
                {
                    result1 = "Motor 1 is On and Speed is 3";
                }

                if (motor2Speed1 == false && motor2Speed2 == false && motor2Speed3 == false)
                {
                    result2 = "Motor 2 is Off";
                }
                else if (motor2Speed1)
                {
                    result2 = "Motor 2 is On and Speed is 1";
                }
                else if (motor2Speed2)
                {
                    result2 = "Motor 2 is On and Speed is 2";
                }
                else if (motor2Speed3)
                {
                    result2 = "Motor 2 is On and Speed is 3";
                }

                string result = string.Format("{0}\r\n{1}", result1, result2);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
                return "Failed";
            }
        }

        public Dictionary<int, int> GetStatus2()
        {
            try
            {
                string motor1Speed1Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor1Speed1_Q";
                string motor1Speed2Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor1Speed2_Q";
                string motor1Speed3Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor1Speed3_Q";
                string motor2Speed1Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor2Speed1_Q";
                string motor2Speed2Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor2Speed2_Q";
                string motor2Speed3Path = @"\\localhost\Simulation\OPC\Channel1\Device1\TechnicalFanCoilMotor2Speed3_Q";

                bool motor1Speed1;
                bool motor1Speed2;
                bool motor1Speed3;
                bool motor2Speed1;
                bool motor2Speed2;
                bool motor2Speed3;

                var subscriberBool1 = new NetworkVariableBufferedSubscriber<bool>(motor1Speed1Path);
                subscriberBool1.Connect();
                var variable1 = subscriberBool1.ReadData();
                motor1Speed1 = variable1.GetValue();
                subscriberBool1.Disconnect();

                var subscriberBool2 = new NetworkVariableBufferedSubscriber<bool>(motor1Speed2Path);
                subscriberBool2.Connect();
                var variable2 = subscriberBool2.ReadData();
                motor1Speed2 = variable2.GetValue();
                subscriberBool2.Disconnect();

                var subscriberBool3 = new NetworkVariableBufferedSubscriber<bool>(motor1Speed3Path);
                subscriberBool3.Connect();
                var variable3 = subscriberBool3.ReadData();
                motor1Speed3 = variable3.GetValue();
                subscriberBool3.Disconnect();

                var subscriberBool4 = new NetworkVariableBufferedSubscriber<bool>(motor2Speed1Path);
                subscriberBool4.Connect();
                var variable4 = subscriberBool4.ReadData();
                motor2Speed1 = variable4.GetValue();
                subscriberBool4.Disconnect();

                var subscriberBool5 = new NetworkVariableBufferedSubscriber<bool>(motor2Speed2Path);
                subscriberBool5.Connect();
                var variable5 = subscriberBool5.ReadData();
                motor2Speed2 = variable5.GetValue();
                subscriberBool5.Disconnect();

                var subscriberBool6 = new NetworkVariableBufferedSubscriber<bool>(motor2Speed3Path);
                subscriberBool6.Connect();
                var variable6 = subscriberBool6.ReadData();
                motor2Speed3 = variable6.GetValue();
                subscriberBool6.Disconnect();

                Dictionary<int,int> result=new Dictionary<int, int>();

                if (motor1Speed1 == false && motor1Speed2 == false && motor1Speed3 == false)
                {
                    result.Add(1,0);
                }
                else if (motor1Speed1)
                {
                    result.Add(1, 1);
                }
                else if (motor1Speed2)
                {
                    result.Add(1, 2);
                }
                else if (motor1Speed3)
                {
                    result.Add(1, 3);
                }

                if (motor2Speed1 == false && motor2Speed2 == false && motor2Speed3 == false)
                {
                    result.Add(2, 0);
                }
                else if (motor2Speed1)
                {
                    result.Add(2, 1);
                }
                else if (motor2Speed2)
                {
                    result.Add(2, 2);
                }
                else if (motor2Speed3)
                {
                    result.Add(2, 3);
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
                return null;
            }
        }
    }
}
