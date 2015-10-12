using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BACnet;

namespace HVACHost
{
    class Program
    {

        private static BACnetStack BACStack = null;
        
        static void Main(string[] args)
        {
            IPAddress ISP01=IPAddress.Parse("192.168.1.30");
            IPAddress ISP02 = IPAddress.Parse("192.168.1.31");
            IPAddress ISP03 = IPAddress.Parse("192.168.1.32");
            List<Property> ISP01Properties=new List<Property>();
            List<Property> ISP02Properties = new List<Property>();
            List<Property> ISP03Properties = new List<Property>();

            BACStack = new BACnetStack();
            
            Device device1=new Device("Device",0,0,new IPEndPoint(ISP01,47808),0,3);
            Device device2 = new Device("Device", 0, 0, new IPEndPoint(ISP02, 47808), 0, 4);
            Device device3 = new Device("Device", 0, 0, new IPEndPoint(ISP03, 47808), 0, 5);

            //if (device1 != null)
            //{
            //    ReadProperties(device1, ISP01Properties);
            //}

            //if (device1 != null)
            //{
            //    ReadProperties(device2, ISP02Properties);
            //}

            //if (device1 != null)
            //{
            //    ReadProperties(device3, ISP03Properties);
            //}

            //var value= ReadValue(device1, BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_BINARY_OUTPUT, 56);

            //Console.WriteLine(value);

            Console.WriteLine(ReadValue(device1, BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_ANALOG_INPUT, 9,BACnetEnums.BACNET_PROPERTY_ID.PROP_OBJECT_NAME));
            Console.WriteLine(ReadValue(device1, BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_ANALOG_INPUT, 9));
            //Console.WriteLine(ReadValue(device1, BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_ANALOG_OUTPUT, 2));
            //Console.WriteLine(ReadValue(device1, BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_ANALOG_OUTPUT, 5));
            //Console.WriteLine(ReadValue(device1, BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_ANALOG_OUTPUT, 8));
            //Console.WriteLine(ReadValue(device1, BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_ANALOG_OUTPUT, 1));
            //Console.WriteLine(ReadValue(device1, BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_ANALOG_OUTPUT, 4));
            


            Console.ReadKey();
        }

        public static void ReadProperties(Device device,List<Property> properties)
        {
            Property property = new Property();
            property.Tag = BACnetEnums.BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_ENUMERATED;
            if (!BACStack.SendReadProperty(
              device,
              device.Instance,
              0, // Array[0] is Object Count
              BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_DEVICE,
              BACnetEnums.BACNET_PROPERTY_ID.PROP_OBJECT_LIST,
              property))
            {
                return;
            }

            if (property.Tag != BACnetEnums.BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_UNSIGNED_INT)
            {
                return;
            }

            int i, tries;
            uint total = property.ValueUInt;
            properties.Clear();
            if (total > 0)
                for (i = 1; i <= total; i++)
                {
                    // Read through Array[x] up to Object Count
                    // Need to try the read again if it times out
                    tries = 0;
                    while (tries < 5)
                    {
                        Console.WriteLine("Device : {0} , Property : {1}", device.ServerEP.Address.ToString(), i);

                        tries++;
                        if (BACStack.SendReadProperty(
                          device,
                          device.Instance,
                          i, // each array index
                          BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_DEVICE,
                          BACnetEnums.BACNET_PROPERTY_ID.PROP_OBJECT_LIST,
                          property))
                        {
                            tries = 5; // Next object
                            if (property.Tag != BACnetEnums.BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_OBJECT_ID)
                                tries = 5; // continue;
                            
                            properties.Add(property);

                            Console.WriteLine(property.ValueObjectInstance);
                        }
                    }
                }
        }

        public static dynamic ReadValue(Device device,BACnetEnums.BACNET_OBJECT_TYPE bacnetObjectType,uint instance,
            BACnetEnums.BACNET_PROPERTY_ID bacnetPropertyId=BACnetEnums.BACNET_PROPERTY_ID.PROP_PRESENT_VALUE)
        {
            BACnetEnums.BACNET_OBJECT_TYPE objtype = bacnetObjectType;
            BACnetEnums.BACNET_PROPERTY_ID propid = bacnetPropertyId;

            uint objinst = instance;
            Property property = new Property();

            if (!BACStack.SendReadProperty(device,
                (uint)objinst,
                -1,
                objtype,
                propid,
                property))
            {
                return -1;
            }

            return property;
        }
    }
}
