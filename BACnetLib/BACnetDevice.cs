using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnetLib
{
    public class BACnetDevice
    {
        private static BACnetDevice instance = null;
        private static readonly object padlock = new object();
        private BACnetStack BACStack = null;

        public static BACnetDevice Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new BACnetDevice();
                        }
                    }
                }
                return instance;
            }
        }
        private BACnetDevice()
        {
            BACStack=new BACnetStack();
        }

        public void ReadProperties(Device device, List<Property> properties)
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

        public dynamic ReadValue(Device device, BACnetEnums.BACNET_OBJECT_TYPE bacnetObjectType, uint instance,
            BACnetEnums.BACNET_PROPERTY_ID bacnetPropertyId = BACnetEnums.BACNET_PROPERTY_ID.PROP_PRESENT_VALUE)
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
