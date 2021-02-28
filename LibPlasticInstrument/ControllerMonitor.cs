using System;
using System.ComponentModel;
using System.Threading;

namespace LibPlasticInstrument
{
  public class ControllerMonitor : IDisposable
  {
    private Thread thread;
    private uint controllerIndex;
    private bool cancel;

    private XInput.GamepadEx lastState;

    public delegate void ControllerEvent(XInput.GamepadEx state);
    public event ControllerEvent OnStateChanged;

    public delegate void DisconnectEvent();
    public event DisconnectEvent OnDisconnect;

    public bool Connected { get; private set; }

    public ControllerMonitor(Controller c)
    {
      var state = new XInput.StateEx();
      if (0 != XInput.XInputGetStateEx(c.Index, ref state))
      {
        throw new Exception($"Controller {c.Index} is not connected");
      }
      lastState = state.Gamepad;
      cancel = false;
      controllerIndex = c.Index;
      Connected = true;
      thread = new Thread(PollThread) { Name = $"Controller {c.Index} poll thread" };
      thread.Start();
    }

    private void PollThread()
    {
      var sleepTime = TimeSpan.FromMilliseconds(0.5);
      while (!cancel)
      {
        if (!Connected)
        {
          // Do less while waiting for the controller to come back online.
          Thread.Sleep(250);
          continue;
        }
        var state = new XInput.StateEx();
        if (0 != XInput.XInputGetStateEx(controllerIndex, ref state))
        {
          Connected = false;
          OnDisconnect?.ThreadSafeInvoke();
          continue;
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
          OnStateChanged.ThreadSafeInvoke(lastState);
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
