using Midi.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibPlasticInstrument
{
  class KeytarTranslator : ITranslator
  {
    private byte[] keyState = new byte[25];
    private byte[] velocityState = new byte[25];
    private bool sustainState = false;
    private int offset = 60;

    public delegate void BasePitchChangeEvent(Midi.Enums.Pitch pitch);

    public event BasePitchChangeEvent OnBasePitchChanged;
    public event NoteOnEvent OnNoteOn;
    public event NoteOffEvent OnNoteOff;
    public event ControlChangeEvent OnControlChange;

    public Midi.Enums.Channel Channel { get; set; } = Midi.Enums.Channel.Channel1;
    public void ControllerEventHandler(XInput.GamepadEx state)
    {
      var (keys, sustain, velocities) = GamepadToKeyState(state);
      if (state.wButtons.HasFlag(XInput.Buttons.DPAD_LEFT))
      {
        offset = Math.Max(offset - 12, 0);
        OnBasePitchChanged.ThreadSafeInvoke((Midi.Enums.Pitch)offset);
      }
      else if (state.wButtons.HasFlag(XInput.Buttons.DPAD_RIGHT))
      {
        offset = Math.Min(offset + 12, 127);
        OnBasePitchChanged.ThreadSafeInvoke((Midi.Enums.Pitch)offset);
      }
      SendMessages(keys, sustain, velocities);
      sustainState = sustain;
    }

    public static (bool[] keys, bool sustain, byte[] velocities) GamepadToKeyState(XInput.GamepadEx state)
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
#if DEBUG
      Console.WriteLine(
        $"Keys: {Convert.ToString(appended, 2).PadLeft(25, '0')} " +
        $"Sustain: {(sustain ? 1 : 0)} " +
        $"Velocities: {velocities.Select(x => x.ToString().PadLeft(3)).Aggregate((x, y) => x + " " + y)} " +
        $"State: {(int)state.wButtons:X4}");
#endif
      return (keys, sustain, velocities);
    }

    void SendMessages(bool[] newKeys, bool sustain, byte[] velocities)
    {
      int newVelocityStart = 0;
      int finalVelocity = 0;
      for (int i = 0; i < 5; i++)
      {
        if (velocities[i] != 0)
        {
          finalVelocity = i;
          if (velocityState[i] == velocities[i])
          {
            newVelocityStart = i + 1;
          }
        }
        velocityState[i] = velocities[i];
      }
      for (int i = 0; i < 25; i++)
      {
        var pitch = (Midi.Enums.Pitch)(i + offset);
        if (newKeys[i] && (0 == keyState[i]))
        {
          OnNoteOn?.Invoke(Channel, pitch, velocities[Math.Min(newVelocityStart, finalVelocity)]);
          keyState[i] = velocities[Math.Min(newVelocityStart, finalVelocity)];
          newVelocityStart++;
        }
        else if (!newKeys[i] && (0 != keyState[i]))
        {
          OnNoteOff?.Invoke(Channel, pitch);
          keyState[i] = 0;
        }
      }
      if (sustain && !sustainState)
        OnControlChange?.Invoke(Channel, Midi.Enums.Control.SustainPedal, 127);
      else if (!sustain && sustainState)
        OnControlChange?.Invoke(Channel, Midi.Enums.Control.SustainPedal, 0);
    }
  }
}
