namespace GamepadMidi
{
  partial class MainWin
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
      this.batteryInfo = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.controllers = new System.Windows.Forms.ComboBox();
      this.baseNoteLabel = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.midiDevices = new System.Windows.Forms.ComboBox();
      this.reloadButton = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // batteryInfo
      // 
      this.batteryInfo.AutoSize = true;
      this.batteryInfo.Location = new System.Drawing.Point(101, 34);
      this.batteryInfo.Name = "batteryInfo";
      this.batteryInfo.Size = new System.Drawing.Size(78, 13);
      this.batteryInfo.TabIndex = 15;
      this.batteryInfo.Text = "Not connected";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(41, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(54, 13);
      this.label3.TabIndex = 14;
      this.label3.Text = "Controller:";
      this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // controllers
      // 
      this.controllers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.controllers.FormattingEnabled = true;
      this.controllers.Location = new System.Drawing.Point(104, 6);
      this.controllers.Name = "controllers";
      this.controllers.Size = new System.Drawing.Size(127, 21);
      this.controllers.TabIndex = 13;
      this.controllers.SelectedIndexChanged += new System.EventHandler(this.controllers_SelectedIndexChanged);
      // 
      // baseNoteLabel
      // 
      this.baseNoteLabel.AutoSize = true;
      this.baseNoteLabel.Location = new System.Drawing.Point(101, 83);
      this.baseNoteLabel.Name = "baseNoteLabel";
      this.baseNoteLabel.Size = new System.Drawing.Size(20, 13);
      this.baseNoteLabel.TabIndex = 12;
      this.baseNoteLabel.Text = "C4";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(35, 83);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(60, 13);
      this.label2.TabIndex = 11;
      this.label2.Text = "Base Note:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 60);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(79, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "Output Device:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // midiDevices
      // 
      this.midiDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.midiDevices.FormattingEnabled = true;
      this.midiDevices.Location = new System.Drawing.Point(104, 57);
      this.midiDevices.Name = "midiDevices";
      this.midiDevices.Size = new System.Drawing.Size(175, 21);
      this.midiDevices.TabIndex = 9;
      this.midiDevices.SelectedIndexChanged += new System.EventHandler(this.midiDevices_SelectedIndexChanged);
      // 
      // reloadButton
      // 
      this.reloadButton.Location = new System.Drawing.Point(237, 5);
      this.reloadButton.Name = "reloadButton";
      this.reloadButton.Size = new System.Drawing.Size(101, 23);
      this.reloadButton.TabIndex = 16;
      this.reloadButton.Text = "Reload Devices";
      this.reloadButton.UseVisualStyleBackColor = true;
      this.reloadButton.Click += new System.EventHandler(this.reloadButton_Click);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(55, 34);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(40, 13);
      this.label4.TabIndex = 17;
      this.label4.Text = "Status:";
      this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // MainWin
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(440, 114);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.reloadButton);
      this.Controls.Add(this.batteryInfo);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.controllers);
      this.Controls.Add(this.baseNoteLabel);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.midiDevices);
      this.Name = "MainWin";
      this.ShowIcon = false;
      this.Text = "Gamepad to MIDI";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.Label batteryInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox controllers;
        private System.Windows.Forms.Label baseNoteLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox midiDevices;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.Label label4;
  }
}

