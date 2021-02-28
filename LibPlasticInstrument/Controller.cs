using System;
using System.Collections.Generic;
using System.Text;

namespace LibPlasticInstrument
{
  public class Controller
  {
    public XInput.Capabilities Capabilities { get; }
    public uint Index { get; }

    internal Controller(uint index, XInput.Capabilities caps)
    {
      Capabilities = caps;
      Index = index;
    }

    public override string ToString()
    {
      return $"{Index}: {Capabilities.SubType}";
    }
  }
}
