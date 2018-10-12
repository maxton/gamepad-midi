using System;
using System.ComponentModel;
using System.Threading;

namespace KeytarPoller
{

  public class ControllerMonitor : IDisposable
  {
    private Thread thread;
    private uint controllerIndex;
    private bool cancel;

    private XInput.XINPUT_GAMEPAD_EX lastState;

    public delegate void ControllerEvent(XInput.XINPUT_GAMEPAD_EX state);
    public event ControllerEvent OnStateChanged;

    public delegate void DisconnectEvent();
    public event DisconnectEvent OnDisconnect;

    public ControllerMonitor(uint controllerIdx)
    {
      var state = new XInput.XINPUT_STATE_EX();
      if (0 != XInput.XInputGetStateEx(controllerIdx, ref state))
      {
        throw new Exception($"Controller {controllerIdx} is not connected");
      }
      lastState = state.Gamepad;
      cancel = false;
      controllerIndex = controllerIdx;

      thread = new Thread(PollThread);
      thread.Start();
    }

    private void PollThread()
    {
      var sleepTime = TimeSpan.FromMilliseconds(0.5);
      while (!cancel)
      {
        var state = new XInput.XINPUT_STATE_EX();
        if (0 != XInput.XInputGetStateEx(controllerIndex, ref state))
        {
          OnDisconnect?.Invoke();
          return;
        }
        var gp = state.Gamepad;
        if (gp.bLeftTrigger != lastState.bLeftTrigger
            || gp.bRightTrigger != lastState.bRightTrigger
            || gp.sThumbLX != lastState.sThumbLX
            || gp.sThumbLY != lastState.sThumbLY
            || gp.sThumbRX != lastState.sThumbRX
            || gp.sThumbRY != lastState.sThumbRY
            || gp.wButtons != lastState.wButtons)
        {
          lastState = gp;
          // https://stackoverflow.com/a/1698918
          foreach (Delegate d in OnStateChanged.GetInvocationList())
          {
            ISynchronizeInvoke syncer = d.Target as ISynchronizeInvoke;
            if (syncer == null)
            {
              d.DynamicInvoke(lastState);
            }
            else
            {
              syncer.BeginInvoke(d, new object[] { lastState });  // cleanup omitted
            }
          }
        }
        Thread.Sleep(sleepTime);
      }
    }

    public void Dispose()
    {
      cancel = true;
      thread.Join();
    }
  }
}
