using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Midi.Enums;

namespace LibPlasticInstrument
{
  public class ProGuitarTranslator : ITranslator
  {
    public event NoteOnEvent OnNoteOn;
    public event NoteOffEvent OnNoteOff;
    public event ControlChangeEvent OnControlChange;

    private byte[] lastPitches = new byte[6];
    private byte[] newPitches = new byte[6];
    public void ControllerEventHandler(XInput.GamepadEx state)
    {
      var newState = ProcessState(state);
      if (lastState == null)
      {
        lastState = newState;
        return;
      }
      if (state.wButtons.HasFlag(XInput.Buttons.A))
      {
        OnControlChange?.Invoke(Channel.Channel1, Control.AllNotesOff, 0);
      }
      for (int i = 0; i < 6; i++)
      {
        if (lastState.Velocities[i] != newState.Velocities[i])
        {
          newPitches[i] = (byte)(newState.Frets[i] + StandardTuning[i]);
          OnNoteOff?.Invoke(Channel.Channel1, (Pitch)lastPitches[i]);
          OnNoteOn?.Invoke(Channel.Channel1, (Pitch)newPitches[i], newState.Velocities[i]);
        }
      }
      // swap note buffers
      var temp = lastPitches;
      lastPitches = newPitches;
      newPitches = temp;
      // swap guitar state
      lastState = newState;
    }

    private GuitarState lastState;
    class GuitarState
    {
      public int[] Frets;
      public int[] Velocities;
      public void Print()
      {
        for (int i = 0; i < 6; i++)
        {
          Console.WriteLine("{0}: {1} {2}", StringNames[i], Frets[i], Velocities[i]);
        }
      }
    }
    private static string[] StringNames = new[]
    {
      "E", "A", "D", "G", "B", "e"
    };
    private static Pitch[] StandardTuning = new[]
    {
      Pitch.E2, Pitch.A2, Pitch.D3, Pitch.G3, Pitch.B3, Pitch.E4
    };
    private static Func<XInput.GamepadEx, int>[] FretFunctions = new Func<XInput.GamepadEx, int>[]
    {
      s =>   s.bLeftTrigger & 0b00011111,
      s => ((s.bLeftTrigger & 0b11100000) >> 5) |
          ((s.bRightTrigger & 0b00000011) << 3),
      s => (s.bRightTrigger & 0b01111100) >> 2,
      s => (s.sThumbLX & 0b0000000000011111),
      s => (s.sThumbLX & 0b0000001111100000) >> 5,
      s => (s.sThumbLX & 0b0111110000000000) >> 10,
    };
    private static Func<XInput.GamepadEx, int>[] VelocityFunctions = new Func<XInput.GamepadEx, int>[]
    {
      s => (s.sThumbLY & 0b0000000001111111),
      s => (s.sThumbLY & 0b0111111100000000) >> 8,
      s => (s.sThumbRX & 0b0000000001111111),
      s => (s.sThumbRX & 0b0111111100000000) >> 8,
      s => (s.sThumbRY & 0b0000000001111111),
      s => (s.sThumbRY & 0b0111111100000000) >> 8,
    };
    private static GuitarState ProcessState(XInput.GamepadEx state)
    {
      return new GuitarState
      {
        Frets = FretFunctions.Select(x => x(state)).ToArray(),
        Velocities = VelocityFunctions.Select(x => x(state)).ToArray()
      };
    }
  }
}
