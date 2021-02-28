using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LibPlasticInstrument
{
  public static class EventExtensions
  {
    // https://stackoverflow.com/a/1698918
    public static void ThreadSafeInvoke(this MulticastDelegate md, params object[] args)
    {
      if (md == null) return;
      foreach (Delegate d in md.GetInvocationList())
      {
        ISynchronizeInvoke syncer = d.Target as ISynchronizeInvoke;
        if (syncer == null)
        {
          d.DynamicInvoke(args);
        }
        else
        {
          syncer.BeginInvoke(d, args);  // cleanup omitted
        }
      }
    }
  }
}
