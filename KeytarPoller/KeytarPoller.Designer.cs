namespace KeytarPoller
{
  partial class KeytarPoller
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
      this.midiDevices = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.baseNoteLabel = new System.Windows.Forms.Label();
      this.controllers = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // midiDevices
      // 
      this.midiDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.midiDevices.FormattingEnabled = true;
      this.midiDevices.Location = new System.Drawing.Point(97, 35);
      this.midiDevices.Name = "midiDevices";
      this.midiDevices.Size = new System.Drawing.Size(175, 21);
      this.midiDevices.TabIndex = 2;
      this.midiDevices.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 38);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(79, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Output Device:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(28, 61);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(60, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Base Note:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // baseNoteLabel
      // 
      this.baseNoteLabel.AutoSize = true;
      this.baseNoteLabel.Location = new System.Drawing.Point(94, 61);
      this.baseNoteLabel.Name = "baseNoteLabel";
      this.baseNoteLabel.Size = new System.Drawing.Size(20, 13);
      this.baseNoteLabel.TabIndex = 5;
      this.baseNoteLabel.Text = "C4";
      // 
      // controllers
      // 
      this.controllers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.controllers.FormattingEnabled = true;
      this.controllers.Location = new System.Drawing.Point(97, 8);
      this.controllers.Name = "controllers";
      this.controllers.Size = new System.Drawing.Size(52, 21);
      this.controllers.TabIndex = 6;
      this.controllers.SelectedIndexChanged += new System.EventHandler(this.controllers_SelectedIndexChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(34, 11);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(54, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Controller:";
      this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // KeytarPoller
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(280, 77);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.controllers);
      this.Controls.Add(this.baseNoteLabel);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.midiDevices);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(300, 120);
      this.MinimumSize = new System.Drawing.Size(300, 120);
      this.Name = "KeytarPoller";
      this.ShowIcon = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "KeytarPoller";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.ComboBox midiDevices;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label baseNoteLabel;
    private System.Windows.Forms.ComboBox controllers;
    private System.Windows.Forms.Label label3;
  }
}

