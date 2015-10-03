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

namespace BACnetTest
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.BroadcastLabel = new System.Windows.Forms.Label();
      this.DeviceList = new System.Windows.Forms.ListBox();
      this.TestWriteLabel = new System.Windows.Forms.Label();
      this.TestBinaryOffBtn = new System.Windows.Forms.Button();
      this.TestBinaryOnBtn = new System.Windows.Forms.Button();
      this.GetObjectsBtn = new System.Windows.Forms.Button();
      this.ObjectList = new System.Windows.Forms.ListBox();
      this.label1 = new System.Windows.Forms.Label();
      this.ObjectLabel = new System.Windows.Forms.Label();
      this.PresentValueLabel = new System.Windows.Forms.Label();
      this.ReadPresentValueBtn = new System.Windows.Forms.Button();
      this.DeviceLabel = new System.Windows.Forms.Label();
      this.GetDevicesBtn = new System.Windows.Forms.Button();
      this.ObjectListLabel = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.NetworkLabel = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.AddressLabel = new System.Windows.Forms.Label();
      this.EndPointLabel = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // BroadcastLabel
      // 
      this.BroadcastLabel.AutoSize = true;
      this.BroadcastLabel.Location = new System.Drawing.Point(9, 35);
      this.BroadcastLabel.Name = "BroadcastLabel";
      this.BroadcastLabel.Size = new System.Drawing.Size(46, 13);
      this.BroadcastLabel.TabIndex = 58;
      this.BroadcastLabel.Text = "Devices";
      // 
      // DeviceList
      // 
      this.DeviceList.FormattingEnabled = true;
      this.DeviceList.Location = new System.Drawing.Point(12, 51);
      this.DeviceList.Name = "DeviceList";
      this.DeviceList.Size = new System.Drawing.Size(155, 329);
      this.DeviceList.TabIndex = 57;
      this.DeviceList.SelectedIndexChanged += new System.EventHandler(this.DeviceList_SelectedIndexChanged);
      // 
      // TestWriteLabel
      // 
      this.TestWriteLabel.AutoSize = true;
      this.TestWriteLabel.Location = new System.Drawing.Point(552, 133);
      this.TestWriteLabel.Name = "TestWriteLabel";
      this.TestWriteLabel.Size = new System.Drawing.Size(56, 13);
      this.TestWriteLabel.TabIndex = 63;
      this.TestWriteLabel.Text = "Test Write";
      // 
      // TestBinaryOffBtn
      // 
      this.TestBinaryOffBtn.Location = new System.Drawing.Point(625, 107);
      this.TestBinaryOffBtn.Name = "TestBinaryOffBtn";
      this.TestBinaryOffBtn.Size = new System.Drawing.Size(64, 23);
      this.TestBinaryOffBtn.TabIndex = 62;
      this.TestBinaryOffBtn.Text = "Test Off";
      this.TestBinaryOffBtn.UseVisualStyleBackColor = true;
      this.TestBinaryOffBtn.Click += new System.EventHandler(this.TestBinaryOffBtn_Click);
      // 
      // TestBinaryOnBtn
      // 
      this.TestBinaryOnBtn.Location = new System.Drawing.Point(555, 107);
      this.TestBinaryOnBtn.Name = "TestBinaryOnBtn";
      this.TestBinaryOnBtn.Size = new System.Drawing.Size(64, 23);
      this.TestBinaryOnBtn.TabIndex = 61;
      this.TestBinaryOnBtn.Text = "Test On";
      this.TestBinaryOnBtn.UseVisualStyleBackColor = true;
      this.TestBinaryOnBtn.Click += new System.EventHandler(this.TestBinaryOnBtn_Click);
      // 
      // GetObjectsBtn
      // 
      this.GetObjectsBtn.Location = new System.Drawing.Point(383, 9);
      this.GetObjectsBtn.Name = "GetObjectsBtn";
      this.GetObjectsBtn.Size = new System.Drawing.Size(102, 23);
      this.GetObjectsBtn.TabIndex = 70;
      this.GetObjectsBtn.Text = "Get Objects";
      this.GetObjectsBtn.UseVisualStyleBackColor = true;
      this.GetObjectsBtn.Click += new System.EventHandler(this.GetObjectsBtn_Click);
      // 
      // ObjectList
      // 
      this.ObjectList.FormattingEnabled = true;
      this.ObjectList.Location = new System.Drawing.Point(383, 51);
      this.ObjectList.Name = "ObjectList";
      this.ObjectList.Size = new System.Drawing.Size(155, 329);
      this.ObjectList.TabIndex = 71;
      this.ObjectList.SelectedIndexChanged += new System.EventHandler(this.ObjectList_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(380, 35);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(43, 13);
      this.label1.TabIndex = 72;
      this.label1.Text = "Objects";
      // 
      // ObjectLabel
      // 
      this.ObjectLabel.AutoSize = true;
      this.ObjectLabel.Location = new System.Drawing.Point(552, 30);
      this.ObjectLabel.Name = "ObjectLabel";
      this.ObjectLabel.Size = new System.Drawing.Size(38, 13);
      this.ObjectLabel.TabIndex = 74;
      this.ObjectLabel.Text = "Object";
      // 
      // PresentValueLabel
      // 
      this.PresentValueLabel.AutoSize = true;
      this.PresentValueLabel.Location = new System.Drawing.Point(552, 77);
      this.PresentValueLabel.Name = "PresentValueLabel";
      this.PresentValueLabel.Size = new System.Drawing.Size(73, 13);
      this.PresentValueLabel.TabIndex = 75;
      this.PresentValueLabel.Text = "Present Value";
      // 
      // ReadPresentValueBtn
      // 
      this.ReadPresentValueBtn.Location = new System.Drawing.Point(555, 51);
      this.ReadPresentValueBtn.Name = "ReadPresentValueBtn";
      this.ReadPresentValueBtn.Size = new System.Drawing.Size(134, 23);
      this.ReadPresentValueBtn.TabIndex = 76;
      this.ReadPresentValueBtn.Text = "Read Present Value";
      this.ReadPresentValueBtn.UseVisualStyleBackColor = true;
      this.ReadPresentValueBtn.Click += new System.EventHandler(this.ReadPresentValueBtn_Click);
      // 
      // DeviceLabel
      // 
      this.DeviceLabel.AutoSize = true;
      this.DeviceLabel.Location = new System.Drawing.Point(186, 51);
      this.DeviceLabel.Name = "DeviceLabel";
      this.DeviceLabel.Size = new System.Drawing.Size(55, 13);
      this.DeviceLabel.TabIndex = 79;
      this.DeviceLabel.Text = "Device ID";
      // 
      // GetDevicesBtn
      // 
      this.GetDevicesBtn.Location = new System.Drawing.Point(12, 9);
      this.GetDevicesBtn.Name = "GetDevicesBtn";
      this.GetDevicesBtn.Size = new System.Drawing.Size(102, 23);
      this.GetDevicesBtn.TabIndex = 81;
      this.GetDevicesBtn.Text = "Get Devices";
      this.GetDevicesBtn.UseVisualStyleBackColor = true;
      this.GetDevicesBtn.Click += new System.EventHandler(this.GetDevicesBtn_Click);
      // 
      // ObjectListLabel
      // 
      this.ObjectListLabel.AutoSize = true;
      this.ObjectListLabel.Location = new System.Drawing.Point(186, 107);
      this.ObjectListLabel.Name = "ObjectListLabel";
      this.ObjectListLabel.Size = new System.Drawing.Size(76, 13);
      this.ObjectListLabel.TabIndex = 83;
      this.ObjectListLabel.Text = "ObjectlistLabel";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(186, 153);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(47, 13);
      this.label3.TabIndex = 84;
      this.label3.Text = "Network";
      // 
      // NetworkLabel
      // 
      this.NetworkLabel.AutoSize = true;
      this.NetworkLabel.Location = new System.Drawing.Point(244, 153);
      this.NetworkLabel.Name = "NetworkLabel";
      this.NetworkLabel.Size = new System.Drawing.Size(47, 13);
      this.NetworkLabel.TabIndex = 85;
      this.NetworkLabel.Text = "Network";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(186, 173);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(45, 13);
      this.label4.TabIndex = 86;
      this.label4.Text = "Address";
      // 
      // AddressLabel
      // 
      this.AddressLabel.AutoSize = true;
      this.AddressLabel.Location = new System.Drawing.Point(244, 173);
      this.AddressLabel.Name = "AddressLabel";
      this.AddressLabel.Size = new System.Drawing.Size(45, 13);
      this.AddressLabel.TabIndex = 87;
      this.AddressLabel.Text = "Address";
      // 
      // EndPointLabel
      // 
      this.EndPointLabel.AutoSize = true;
      this.EndPointLabel.Location = new System.Drawing.Point(244, 133);
      this.EndPointLabel.Name = "EndPointLabel";
      this.EndPointLabel.Size = new System.Drawing.Size(53, 13);
      this.EndPointLabel.TabIndex = 89;
      this.EndPointLabel.Text = "End Point";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(186, 133);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(53, 13);
      this.label6.TabIndex = 88;
      this.label6.Text = "End Point";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(708, 391);
      this.Controls.Add(this.EndPointLabel);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.AddressLabel);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.NetworkLabel);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.ObjectListLabel);
      this.Controls.Add(this.GetDevicesBtn);
      this.Controls.Add(this.DeviceLabel);
      this.Controls.Add(this.ReadPresentValueBtn);
      this.Controls.Add(this.PresentValueLabel);
      this.Controls.Add(this.ObjectLabel);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.ObjectList);
      this.Controls.Add(this.GetObjectsBtn);
      this.Controls.Add(this.TestWriteLabel);
      this.Controls.Add(this.TestBinaryOffBtn);
      this.Controls.Add(this.TestBinaryOnBtn);
      this.Controls.Add(this.BroadcastLabel);
      this.Controls.Add(this.DeviceList);
      this.Name = "MainForm";
      this.Text = "BACnet Test";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label BroadcastLabel;
    private System.Windows.Forms.ListBox DeviceList;
    private System.Windows.Forms.Label TestWriteLabel;
    private System.Windows.Forms.Button TestBinaryOffBtn;
    private System.Windows.Forms.Button TestBinaryOnBtn;
    private System.Windows.Forms.Button GetObjectsBtn;
    private System.Windows.Forms.ListBox ObjectList;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label ObjectLabel;
    private System.Windows.Forms.Label PresentValueLabel;
    private System.Windows.Forms.Button ReadPresentValueBtn;
    private System.Windows.Forms.Label DeviceLabel;
    private System.Windows.Forms.Button GetDevicesBtn;
    private System.Windows.Forms.Label ObjectListLabel;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label NetworkLabel;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label AddressLabel;
    private System.Windows.Forms.Label EndPointLabel;
    private System.Windows.Forms.Label label6;
  }
}

