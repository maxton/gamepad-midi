using System;
using System.Collections.Generic;
using System.Text;

using BTN = LibPlasticInstrument.XInput.Buttons;

namespace LibPlasticInstrument
{
  public class DrumTranslator : ITranslator
  {
    private Drums drumState;
    public Midi.Enums.Channel Channel { get; set; } = Midi.Enums.Channel.Channel10;

    public event NoteOnEvent OnNoteOn;
    public event NoteOffEvent OnNoteOff;
    public event ControlChangeEvent OnControlChange;

    public void ControllerEventHandler(XInput.GamepadEx state)
    {
      var drums = HitDrums(state.wButtons);
      SendMessages(drums, state);
      drumState = drums;
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
      GreenCymbal = 0x80,
      Kick2 = 0x100
    };
    private Drums HitDrums(BTN b)
    {
      var flags = (Drums)0;
      if (b.HasFlag(BTN.DPAD_UP | BTN.RIGHT_SHOULDER | BTN.Y))
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
      if (b.HasFlag(BTN.LEFT_THUMB))
      {
        flags |= Drums.Kick2;
      }
      return flags;
    }

    private Dictionary<Drums, Func<Drums, byte>> DrumNoteMap = new Dictionary<Drums, Func<Drums, byte>>
    {
      { Drums.Kick, _ => 36 },
      { Drums.Kick2, _ => 44 },
      { Drums.RedDrum, _ => 38 },
      { Drums.YellowDrum, _ => 48 },
      { Drums.BlueDrum, _ => 47 },
      { Drums.GreenDrum, _ => 43 },
      { Drums.YellowCymbal, s => s.HasFlag(Drums.Kick2) ? (byte)42 : (byte)46 },
      { Drums.BlueCymbal, _ => 51 },
      { Drums.GreenCymbal, _ => 49 },
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
      return (byte)((thumbData - min) / (max - min) * (bmax - bmin) + bmin);
    }

    private Dictionary<Drums, Func<XInput.GamepadEx, byte>> VelocityFunctions = new Dictionary<Drums, Func<XInput.GamepadEx, byte>>
    {
      { Drums.Kick, x => 80 },
      { Drums.Kick2, x=> 80 },
      { Drums.RedDrum, x =>      Interp(x.sThumbLX, rMin, rMax, 63, 127) },
      { Drums.YellowDrum, x =>   Interp(x.sThumbLY, yMin, yMax, 63, 127) },
      { Drums.BlueDrum, x =>     Interp(x.sThumbRX, bMin, bMax, 63, 127) },
      { Drums.GreenDrum, x =>    Interp(x.sThumbRY, gMin, gMax, 63, 127) },
      { Drums.YellowCymbal, x => Interp(x.sThumbLY, yMin, yMax, 80, 127) },
      { Drums.BlueCymbal, x =>   Interp(x.sThumbRX, bMin, bMax, 63, 127) },
      { Drums.GreenCymbal, x =>  Interp(x.sThumbRY, gMin, gMax, 63, 127) },
    };

    void SendMessages(Drums newState, XInput.GamepadEx state)
    {
      foreach (Drums d in Enum.GetValues(typeof(Drums)))
      {
        var note = DrumNoteMap[d](newState);
        if (newState.HasFlag(d) && !drumState.HasFlag(d))
        {
          var velocity = VelocityFunctions[d](state);
          if (velocity > 127) velocity = 127;
          if (velocity < 32) velocity = 32;
          OnNoteOn?.Invoke(Channel, (Midi.Enums.Pitch)note, velocity);
        }
        else if (!newState.HasFlag(d) && drumState.HasFlag(d))
        {
          OnNoteOff?.Invoke(Channel, (Midi.Enums.Pitch)note);
        }
      }
    }
  }
}
