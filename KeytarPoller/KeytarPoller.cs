using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace KeytarPoller
{
  public partial class KeytarPoller : Form
  {
    private ControllerMonitor mon;
    private Midi.Devices.IOutputDevice device;
    private bool[] keyState;
    private int offset = 60;

    public KeytarPoller()
    {
      InitializeComponent();
      keyState = new bool[25];
      foreach(var device in Midi.Devices.DeviceManager.OutputDevices)
      {
        comboBox1.Items.Add(device.Name);
      }
      //for (uint i = 0; i < XInput.XUSER_MAX_COUNT; i++)
      //{
      //    var caps = new XInput.XINPUT_CAPABILITIES();
      //    if(0 == XInput.XInputGetCapabilities(i, 1, ref caps))
      //      textBox1.AppendText($"Type: {caps.Type} Subtype: {caps.SubType} Flags: {caps.Flags}" + Environment.NewLine);
      //}
    }

    private void button1_Click(object sender, EventArgs e)
    {
      
      if(mon == null)
      {
        mon = new ControllerMonitor(0);
        mon.OnStateChanged += Mon_OnStateChanged;
      }
    }

    private void Mon_OnStateChanged(XInput.XINPUT_GAMEPAD state)
    {
      //textBox1.AppendText(
      //  $"{state.wButtons:X4} | " +
      //  $"{state.bLeftTrigger:X2} | {state.bRightTrigger:X2} | " +
      //  $"{state.sThumbLX:X4} | {state.sThumbLY:X4} | " +
      //  $"{state.sThumbRX:X4} | {state.sThumbRY:X4}" + Environment.NewLine);
      var keys = GamepadToKeyState(state);
      if(state.wButtons.HasFlag(XInput.Buttons.DPAD_LEFT))
      {
        offset = Math.Max(offset - 12, 0);
        baseNoteLabel.Text = Enum.GetName(typeof(Midi.Enums.Pitch), (Midi.Enums.Pitch)offset);
      } else if(state.wButtons.HasFlag(XInput.Buttons.DPAD_RIGHT))
      {
        offset = Math.Min(offset + 12, 127);
        baseNoteLabel.Text = Enum.GetName(typeof(Midi.Enums.Pitch), (Midi.Enums.Pitch)offset);
      }
      SendMessages(keys);
      keyState = keys;
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      mon?.Dispose();
      device?.Close();
    }

    public static bool[] GamepadToKeyState(XInput.XINPUT_GAMEPAD state)
    {
      var ret = new bool[25];
      int test = 1 << 24;
      int key = 0;
      int appended = 
        (state.bLeftTrigger << 17) |
        (state.bRightTrigger << 9) |
        ((state.sThumbLX & 0xFF) << 1) |
        ((state.sThumbLX >> 15) & 0x1);
      for(var i = 0; i < 25; i++)
      {
        int mask = test >> i;
        ret[key++] = (appended & mask) == mask;
      }
      return ret;
    }

    void SendMessages(bool[] newKeys)
    {
      if (device == null) return;
      for(int i = 0; i < 25; i++)
      {
        var pitch = (Midi.Enums.Pitch)(i + offset);
        var channel = Midi.Enums.Channel.Channel1;
        if (newKeys[i] && !keyState[i])
          device.SendNoteOn(channel, pitch, 64);
        else if (!newKeys[i] && keyState[i])
          device.SendNoteOff(channel, pitch, 0);
      }
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      device?.Close();
      device = Midi.Devices.DeviceManager.OutputDevices.Where((d) => d.Name.Equals(comboBox1.SelectedItem)).First();
      device.Open();
    }
  }
}
