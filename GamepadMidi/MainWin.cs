using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibPlasticInstrument;

namespace GamepadMidi
{
  public partial class MainWin : Form
  {
    private ControllerMonitor mon;
    private Midi.Devices.IOutputDevice device;
    private KeytarTranslator keytarTranslator;
    private DrumTranslator drumTranslator;

    public MainWin()
    {
      InitializeComponent();
      RefreshDevices();
      keytarTranslator = new KeytarTranslator();
      keytarTranslator.OnBasePitchChanged += Translator_OnBasePitchChanged;
      keytarTranslator.OnNoteOff += Translator_OnNoteOff;
      keytarTranslator.OnNoteOn += Translator_OnNoteOn;
      keytarTranslator.OnControlChange += Translator_OnControlChange;

      drumTranslator = new DrumTranslator();
      drumTranslator.OnNoteOff += Translator_OnNoteOff;
      drumTranslator.OnNoteOn += Translator_OnNoteOn;
      drumTranslator.OnControlChange += Translator_OnControlChange;
    }

    private void GamepadMidi_FormClosing(object sender, FormClosingEventArgs e)
    {
      mon?.Dispose();
      device?.Close();
    }

    private void RefreshDevices()
    {
      DisposeMonitor();
      controllers.Items.Clear();
      midiDevices.Items.Clear();
      foreach (var device in Midi.Devices.DeviceManager.OutputDevices)
      {
        midiDevices.Items.Add(device.Name);
      }
      foreach (var device in ControllerEnumerator.EnumerateControllers())
      {
        controllers.Items.Add(device);
      }
    }
    private void Controller_OnDisconnect() => RefreshDevices();
    private void reloadButton_Click(object sender, EventArgs e) => RefreshDevices();

    private void DisposeMonitor()
    {
      if (mon == null) return;
      batteryInfo.Text = "Disconnected";
      mon.OnStateChanged -= keytarTranslator.ControllerEventHandler;
      mon.OnStateChanged -= drumTranslator.ControllerEventHandler;
      mon.Dispose();
      mon = null;
    }

    private void Translator_OnBasePitchChanged(Midi.Enums.Pitch pitch)
      => baseNoteLabel.Text = Enum.GetName(typeof(Midi.Enums.Pitch), pitch);
    private void Translator_OnNoteOn(Midi.Enums.Channel c, Midi.Enums.Pitch p, int v)
      => device?.SendNoteOn(c, p, v);
    private void Translator_OnNoteOff(Midi.Enums.Channel c, Midi.Enums.Pitch p)
      => device?.SendNoteOff(c, p, 0);
    private void Translator_OnControlChange(Midi.Enums.Channel c, Midi.Enums.Control ctrl, int v)
      => device?.SendControlChange(c, ctrl, v);

    private void midiDevices_SelectedIndexChanged(object sender, EventArgs e)
    {
      device?.Close();
      device = Midi.Devices.DeviceManager.OutputDevices.Where((d) => d.Name.Equals(midiDevices.SelectedItem)).First();
      device?.Open();
    }

    private void controllers_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (mon != null)
        DisposeMonitor();
      if (controllers.SelectedItem is Controller c)
      {
        mon = new ControllerMonitor(c);
        mon.OnDisconnect += Controller_OnDisconnect;
        if (c.Capabilities.SubType == XInput.DevSubType.Keytar)
        {
          mon.OnStateChanged += keytarTranslator.ControllerEventHandler;
        }
        else if (c.Capabilities.SubType == XInput.DevSubType.DrumKit)
        {
          mon.OnStateChanged += drumTranslator.ControllerEventHandler;
        }
        XInput.BatteryInformation xbi = default;
        XInput.XInputGetBatteryInformation(((Controller)controllers.SelectedItem).Index, XInput.BatteryDevType.Gamepad, ref xbi);
        batteryInfo.Text = $"Connected. Battery: {xbi.BatteryType} {xbi.BatteryLevel}";
      }
    }
  }
}
