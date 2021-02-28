using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibPlasticInstrument
{
  public delegate void NoteOnEvent(Midi.Enums.Channel channel, Midi.Enums.Pitch pitch, int velocity);
  public delegate void NoteOffEvent(Midi.Enums.Channel channel, Midi.Enums.Pitch pitch);
  public delegate void ControlChangeEvent(Midi.Enums.Channel channel, Midi.Enums.Control ctrl, int value);

  public interface ITranslator
  {
    event NoteOnEvent OnNoteOn;
    event NoteOffEvent OnNoteOff;
    event ControlChangeEvent OnControlChange;
    void ControllerEventHandler(XInput.GamepadEx state);
  }
}
