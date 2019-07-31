using System;
using System.Collections.Generic;
using System.Text;

namespace LibPlasticInstrument
{
  public static class ControllerEnumerator
  {
    /// <summary>
    /// Returns an list of controller capabilities.
    /// </summary>
    /// <returns></returns>
    public static List<XInput.XINPUT_CAPABILITIES> EnumerateControllers()
    {
      var ret = new List<XInput.XINPUT_CAPABILITIES>();
      for (uint i = 0; i < XInput.XUSER_MAX_COUNT; i++)
      {
        var caps = new XInput.XINPUT_CAPABILITIES();
        if (0 == XInput.XInputGetCapabilities(i, 1, ref caps))
        {
          ret.Add(caps);
        }
      }
      return ret;
    }
  }
}
