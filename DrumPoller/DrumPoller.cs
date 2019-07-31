using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LibPlasticInstrument;

using BTN = LibPlasticInstrument.XInput.Buttons;

namespace DrumPoller
{
  public partial class DrumPoller : Form
  {
    private ControllerMonitor mon;
    private Midi.Devices.IOutputDevice device;
    private Drums drumState;
    private ushort[] velocityState;

    private int offset = 60;
    const byte subTypeDrumkit = 8;

    public DrumPoller()
    {
      InitializeComponent();
      velocityState = new ushort[25];
      foreach(var device in Midi.Devices.DeviceManager.OutputDevices)
      {
        midiDevices.Items.Add(device.Name);
      }
      var devices = ControllerEnumerator.EnumerateControllers();
      for (int i = 0; i < devices.Count; i++)
      {
        if(devices[i].SubType == subTypeDrumkit)
        {
          controllers.Items.Add((uint)i);
        }
      }
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
      XInput.XINPUT_BATTERY_INFORMATION xbi = default;
      XInput.XInputGetBatteryInformation((uint)controllers.SelectedItem, XInput.BATTERY_DEVTYPE.GAMEPAD, ref xbi);
      label4.Text = $"Battery: {xbi.BatteryType} {xbi.BatteryLevel}";
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      mon?.Dispose();
      device?.Close();
    }

    [Flags]
    enum Drums
    {
      RedDrum = 0x1,
      YellowDrum = 0x2,
      BlueDrum = 0x4,
      GreenDrum = 0x8,
      Kick = 0x10,
      YellowCymbal = 0x20,
      BlueCymbal = 0x40,
      GreenCymbal = 0x80
    };
    private Drums HitDrums(BTN b)
    {
      var flags = (Drums)0;
      if(b.HasFlag(BTN.DPAD_UP | BTN.RIGHT_SHOULDER | BTN.Y))
      {
        flags |= Drums.YellowCymbal;
      }
      if (b.HasFlag(BTN.DPAD_DOWN | BTN.RIGHT_SHOULDER | BTN.X))
      {
        flags |= Drums.BlueCymbal;
      }
      if (b.HasFlag(BTN.RIGHT_SHOULDER | BTN.A))
      {
        flags |= Drums.GreenCymbal;
      }
      if (b.HasFlag(BTN.B | BTN.RIGHT_THUMB))
      {
        flags |= Drums.RedDrum;
      }
      if (b.HasFlag(BTN.Y | BTN.RIGHT_THUMB))
      {
        if (!flags.HasFlag(Drums.YellowCymbal))
          flags |= Drums.YellowDrum;
      }
      if (b.HasFlag(BTN.X | BTN.RIGHT_THUMB))
      {
        if (!flags.HasFlag(Drums.BlueCymbal))
          flags |= Drums.BlueDrum;
      }
      if (b.HasFlag(BTN.A | BTN.RIGHT_THUMB))
      {
        if (!flags.HasFlag(Drums.GreenCymbal))
          flags |= Drums.GreenDrum;
      }
      if (b.HasFlag(BTN.LEFT_SHOULDER))
      {
        flags |= Drums.Kick;
      }
      return flags;
    }
    private void Mon_OnStateChanged(XInput.XINPUT_GAMEPAD_EX state)
    {
      var drums = HitDrums(state.wButtons);
      SendMessages(drums, state);
      drumState = drums;
    }

    private Dictionary<Drums, byte> DrumNoteMap = new Dictionary<Drums, byte>
    {
      { Drums.Kick, 36 },
      { Drums.RedDrum, 38 },
      { Drums.YellowDrum, 48 },
      { Drums.BlueDrum, 47 },
      { Drums.GreenDrum, 43 },
      { Drums.YellowCymbal, 42 },
      { Drums.BlueCymbal, 51 },
      { Drums.GreenCymbal, 49 },
    };

    const double rMax = 7500;
    const double rMin = 32767;
    const double yMax = -7500;
    const double yMin = -32768;
    const double bMax = rMax;
    const double bMin = rMin;
    const double gMax = yMax;
    const double gMin = yMin;
    
    private static byte Interp(short thumbData, double min, double max, byte bmin, byte bmax)
    {
      return (byte)((thumbData - min) / (max - min) * (bmax-bmin) + bmin);
    }

    private Dictionary<Drums, Func<XInput.XINPUT_GAMEPAD_EX, byte>> VelocityFunctions = new Dictionary<Drums, Func<XInput.XINPUT_GAMEPAD_EX, byte>>
    {
      { Drums.Kick, x => 80 },
      { Drums.RedDrum, x =>      Interp(x.sThumbLX, rMin, rMax, 63, 127) },
      { Drums.YellowDrum, x =>   Interp(x.sThumbLY, yMin, yMax, 63, 127) },
      { Drums.BlueDrum, x =>     Interp(x.sThumbRX, bMin, bMax, 63, 127) },
      { Drums.GreenDrum, x =>    Interp(x.sThumbRY, gMin, gMax, 63, 127) },
      { Drums.YellowCymbal, x => Interp(x.sThumbLY, yMin, yMax, 63, 127) },
      { Drums.BlueCymbal, x =>   Interp(x.sThumbRX, bMin, bMax, 63, 127) },
      { Drums.GreenCymbal, x =>  Interp(x.sThumbRY, gMin, gMax, 63, 127) },
    };
    void SendMessages(Drums newState, XInput.XINPUT_GAMEPAD_EX state)
    {
      Console.WriteLine($"{state.sThumbLX} {state.sThumbLY} {state.sThumbRX} {state.sThumbRY}");
      if (device == null) return;
      var channel = Midi.Enums.Channel.Channel10;
      foreach(Drums d in Enum.GetValues(typeof(Drums)))
      {
        var note = DrumNoteMap[d];
        if(newState.HasFlag(d) && !drumState.HasFlag(d))
        {
          var velocity = VelocityFunctions[d](state);
          if (velocity > 127) velocity = 127;
          if (velocity < 32) velocity = 32;
          device.SendNoteOn(channel, (Midi.Enums.Pitch)note, velocity);
        }
        else if(!newState.HasFlag(d) && drumState.HasFlag(d))
        {
          device.SendNoteOff(channel, (Midi.Enums.Pitch)note, 0);
        }
      }
    }
  }
}
