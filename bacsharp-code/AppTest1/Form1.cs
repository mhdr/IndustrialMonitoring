/**************************************************************************
*
* THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
* OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
* CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
* SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*
*********************************************************************/

/* COPYRIGHT
 -------------------------------------------
 Copyright (C) 2013-2015 Plus 1 Micro, Inc.

 This program is free software; you can redistribute it and/or
 modify it under the terms of the GNU General Public License
 as published by the Free Software Foundation; either version 2
 of the License, or (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to:
 The Free Software Foundation, Inc.
 59 Temple Place - Suite 330
 Boston, MA  02111-1307, USA.

 As a special exception, if other files instantiate templates or
 use macros or inline functions from this file, or you compile
 this file and link it with other works to produce a work based
 on this file, this file does not by itself cause the resulting
 work to be covered by the GNU General Public License. However
 the source code for this file must still be made available in
 accordance with section (3) of the GNU General Public License.

 This exception does not invalidate any other reasons why a work
 based on this file might be covered by the GNU General Public
 License.
 -------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Ini;
using BACnet;

namespace BACnetTest
{
  public partial class MainForm : Form
  {

    public static MainForm Self;

    public BACnetStack BACStack = null;
    
    public MainForm()
    {
      InitializeComponent();
      Self = this;  // Works only if there is only one mainform

      GetObjectsBtn.Enabled = false;
      ReadPresentValueBtn.Enabled = false;
      TestBinaryOnBtn.Enabled = false;
      TestBinaryOffBtn.Enabled = false;
      ObjectListLabel.Text = "";

      // Create the BACNet stack
      BACStack = new BACnetStack();

    }

    public void CreateDeviceList()
    {
      BACStack.GetDevices(1000);
      DeviceList.Items.Clear();
      if (BACnetData.Devices.Count == 0)
      {
        MessageBox.Show("No Devices found");
      }
      else
      {
        foreach (Device dev in BACnetData.Devices)
        {
          DeviceList.Items.Add(
            //dev.VendorID.ToString() + ", " + 
            dev.Network.ToString() + ", " +
            dev.Instance.ToString());
        }
      }
    }

    public void SetBroadcastLabel(string s)
    {
      BroadcastLabel.Text = s;
    }

    private void GetDevicesBtn_Click(object sender, EventArgs e)
    {
      CreateDeviceList();
    }

    private void DeviceList_SelectedIndexChanged(object sender, EventArgs e)
    {
      int idx = DeviceList.SelectedIndex;
      if ((idx >= 0) && (idx < BACnetData.Devices.Count()))
      {
        BACnetData.DeviceIndex = idx;
        Device device = BACnetData.Devices[idx];
        EndPointLabel.Text = device.ServerEP.ToString();
        NetworkLabel.Text = device.Network.ToString();
        AddressLabel.Text = device.MACAddress.ToString();
        GetObjectsBtn.Enabled = true;
      }
      else
        GetObjectsBtn.Enabled = false;
    }

    private void GetObjectsBtn_Click(object sender, EventArgs e)
    {
      // Read the Device Array
      ObjectListLabel.Text = "";
      Property property = new Property();
      property.Tag = BACnetEnums.BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_ENUMERATED;
      if (!BACStack.SendReadProperty(
        BACnetData.DeviceIndex,
        BACnetData.Devices[BACnetData.DeviceIndex].Instance,
        0, // Array[0] is Object Count
        BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_DEVICE,
        BACnetEnums.BACNET_PROPERTY_ID.PROP_OBJECT_LIST,
        property))
      {
        ObjectListLabel.Text = "Read Property Object List Error (1)";
        return;
      }

      if (property.Tag != BACnetEnums.BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_UNSIGNED_INT)
      {
        ObjectListLabel.Text = "Read Property Object List Error (2)";
        return;
      }

      ObjectListLabel.Text = property.ValueUInt.ToString() + " objects found";

      int i, tries;
      uint total = property.ValueUInt;
      ObjectList.Items.Clear();
      if (total > 0) for (i = 1; i <= total; i++)
      {
        // Read through Array[x] up to Object Count
        // Need to try the read again if it times out
        tries = 0;
        while (tries < 5)
        {
          tries++;
          if (BACStack.SendReadProperty(
            BACnetData.DeviceIndex,
            BACnetData.Devices[BACnetData.DeviceIndex].Instance,
            i, // each array index
            BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_DEVICE,
            BACnetEnums.BACNET_PROPERTY_ID.PROP_OBJECT_LIST,
            property))
          {
            tries = 5; // Next object
            string s;
            if (property.Tag != BACnetEnums.BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_OBJECT_ID)
              tries = 5; // continue;
            switch (property.ValueObjectType)
            {
              case BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_DEVICE: s = "Device"; break;
              case BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_ANALOG_INPUT: s = "Analog Input"; break;
              case BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_ANALOG_OUTPUT: s = "Analog Output"; break;
              case BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_ANALOG_VALUE: s = "Analog value"; break;
              case BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_BINARY_INPUT: s = "Binary Input"; break;
              case BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_BINARY_OUTPUT: s = "Binary Output"; break;
              case BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_BINARY_VALUE: s = "Binary value"; break;
              default: s = "Other"; break;
            }
            s = s + "  " + property.ValueObjectInstance.ToString();
            ObjectList.Items.Add(s);
          }
        }
      }
    }

    private void ObjectList_SelectedIndexChanged(object sender, EventArgs e)
    {
      int idx = ObjectList.SelectedIndex;
      ReadPresentValueBtn.Enabled = false;
      TestBinaryOnBtn.Enabled = false;
      TestBinaryOffBtn.Enabled = false;
      if (idx >= 0)
      {
        string s = ObjectList.Items[idx].ToString();
        ObjectLabel.Text = s;
        if (s.Length > 13) if (s.Substring(0, 13) == "Binary Output")
        {
          ReadPresentValueBtn.Enabled = true;
          TestBinaryOnBtn.Enabled = true;
          TestBinaryOffBtn.Enabled = true;
        }
      }
    }

    private void ReadPresentValueBtn_Click(object sender, EventArgs e)
    {
      PresentValueLabel.Text = "...";

      // Assume just Binary Output for now ...
      BACnetEnums.BACNET_OBJECT_TYPE objtype = BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_BINARY_OUTPUT;
      BACnetEnums.BACNET_PROPERTY_ID propid = BACnetEnums.BACNET_PROPERTY_ID.PROP_PRESENT_VALUE;
      int idx = ObjectList.SelectedIndex;
      if (idx >= 0)
      {
        string s = ObjectList.Items[idx].ToString();
        string s1 = s.Substring(15);
        if (s1.Length > 0)
        {
          uint objinst = Convert.ToUInt32(s1);
          Property property = new Property();
         
          if (!BACStack.SendReadProperty(BACnetData.DeviceIndex, 
            (uint)objinst, 
            -1, 
            objtype, 
            propid, 
            property))
          {
            PresentValueLabel.Text = "Read Present Value Error (1)";
            return;
          }
          if (property.Tag != BACnetEnums.BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_ENUMERATED)
          {
            PresentValueLabel.Text = "Read Present Value Error (2)";
            return;
          }
          if (property.ValueEnum == 1)
            PresentValueLabel.Text = "Binary Output On";
          else
            PresentValueLabel.Text = "Binary Output Off";
        }
      }
    }

    private void SendBinaryOutput(bool OnState)
    {
      TestWriteLabel.Text = "...";

      // Assume just Binary Output for now ...
      BACnetEnums.BACNET_OBJECT_TYPE objtype = BACnetEnums.BACNET_OBJECT_TYPE.OBJECT_BINARY_OUTPUT;
      BACnetEnums.BACNET_PROPERTY_ID propid = BACnetEnums.BACNET_PROPERTY_ID.PROP_PRESENT_VALUE;
      int idx = ObjectList.SelectedIndex;
      if (idx >= 0)
      {
        string s = ObjectList.Items[idx].ToString();
        string s1 = s.Substring(15);
        if (s1.Length > 0)
        {
          uint objinst = Convert.ToUInt32(s1);
          Property property = new Property();
          property.Tag = BACnetEnums.BACNET_APPLICATION_TAG.BACNET_APPLICATION_TAG_ENUMERATED;
          if (OnState)
            property.ValueEnum = 1; // Turn it on
          else
            property.ValueEnum = 0; // Turn it off
          if (BACStack.SendWriteProperty(
            BACnetData.DeviceIndex,
            (uint)objinst,
            -1,
            objtype,
            propid,
            property,
            1))
            TestWriteLabel.Text = "Binary Output " + (OnState ? "On" : "Off");
          else
            TestWriteLabel.Text = "Binary Output On Error";
        }
      }
    }

    private void TestBinaryOnBtn_Click(object sender, EventArgs e)
    {
      // Test Binary On
      SendBinaryOutput(true);
    }

    private void TestBinaryOffBtn_Click(object sender, EventArgs e)
    {
      // Test Binary Off
      TestWriteLabel.Text = "...";
      SendBinaryOutput(false);
    }

  }

}
