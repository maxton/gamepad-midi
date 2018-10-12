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
    private bool sustainState;

    private int offset = 60;

    public KeytarPoller()
    {
      InitializeComponent();
      keyState = new bool[25];
      foreach(var device in Midi.Devices.DeviceManager.OutputDevices)
      {
        midiDevices.Items.Add(device.Name);
      }
      for (uint i = 0; i < XInput.XUSER_MAX_COUNT; i++)
      {
        var caps = new XInput.XINPUT_CAPABILITIES();
        if(0 == XInput.XInputGetCapabilities(i, 1, ref caps))
        {
          if(caps.SubType == 15)
          {
            controllers.Items.Add(i);
          }
        }
      }
    }

    private void Mon_OnStateChanged(XInput.XINPUT_GAMEPAD_EX state)
    {
      var (keys, sustain, _) = GamepadToKeyState(state);
      if(state.wButtons.HasFlag(XInput.Buttons.DPAD_LEFT))
      {
        offset = Math.Max(offset - 12, 0);
        baseNoteLabel.Text = Enum.GetName(typeof(Midi.Enums.Pitch), (Midi.Enums.Pitch)offset);
      } else if(state.wButtons.HasFlag(XInput.Buttons.DPAD_RIGHT))
      {
        offset = Math.Min(offset + 12, 127);
        baseNoteLabel.Text = Enum.GetName(typeof(Midi.Enums.Pitch), (Midi.Enums.Pitch)offset);
      }
      SendMessages(keys, sustain);
      keyState = keys;
      sustainState = sustain;
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      mon?.Dispose();
      device?.Close();
    }

    public static (bool[], bool, byte[]) GamepadToKeyState(XInput.XINPUT_GAMEPAD_EX state)
    {
      bool sustain = (state.sThumbRY & 0x80) == 0x80;
      var velocities = new byte[] {
        (byte)(state.sThumbLX >> 8 & 0x7F),
        (byte)(state.sThumbLY & 0x7F),
        (byte)(state.sThumbLY >> 8 & 0x7F),
        (byte)(state.sThumbRX & 0x7F),
        (byte)(state.sThumbRX >> 8 & 0x7F),
      };

      var keys = new bool[25];
      int appended =
        (state.bLeftTrigger << 17) |
        (state.bRightTrigger << 9) |
        ((state.sThumbLX & 0xFF) << 1) |
        ((state.sThumbLX >> 15) & 0x1);
      int test = 1 << 24;
      int key = 0;
      for (var i = 0; i < 25; i++)
      {
        int mask = test >> i;
        keys[key++] = (appended & mask) == mask;
      }

      // Console.WriteLine(
      //   $"Keys: {Convert.ToString(appended, 2).PadLeft(25, '0')} " +
      //   $"Sustain: {(sustain ? 1 : 0)} " +
      //   $"Velocities: {velocities.Select(x => x.ToString().PadLeft(3)).Aggregate((x, y) => x + " " + y)} " +
      //   $"State: {(int)state.wButtons:X4}");

      return (keys, sustain, velocities);
    }

    void SendMessages(bool[] newKeys, bool sustain)
    {
      if (device == null) return;

      var channel = Midi.Enums.Channel.Channel1;
      for (int i = 0; i < 25; i++)
      {
        var pitch = (Midi.Enums.Pitch)(i + offset);
        if (newKeys[i] && !keyState[i])
          device.SendNoteOn(channel, pitch, 64);
        else if (!newKeys[i] && keyState[i])
          device.SendNoteOff(channel, pitch, 0);
      }
      if (sustain && !sustainState)
        device.SendControlChange(channel, Midi.Enums.Control.SustainPedal, 127);
      else if (!sustain && sustainState)
        device.SendControlChange(channel, Midi.Enums.Control.SustainPedal, 0);
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      device?.Close();
      device = Midi.Devices.DeviceManager.OutputDevices.Where((d) => d.Name.Equals(midiDevices.SelectedItem)).First();
      device?.Open();
    }

    private void controllers_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (mon != null)
      {
        mon.Dispose();
      }
      mon = new ControllerMonitor((uint)controllers.SelectedItem);
      mon.OnStateChanged += Mon_OnStateChanged;
    }
  }
}
