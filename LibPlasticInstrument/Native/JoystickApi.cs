using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace LibPlasticInstrument
{
  public static class JoystickApi
  {
    [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern int joyGetNumDevs();

    [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern int joyGetPosEx(int uJoyID, ref JoyInfoEx pji);

    [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern int joyGetDevCaps(int uJoyID, ref JoyCaps caps, int cbjc);

    /// <summary>
    /// Contains extended information about the joystick position, point-of-view position, and button state.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct JoyInfoEx
    {
      /// <summary>
      /// Size, in bytes, of this structure. Use System.Runtime.InteropServices.Marshal.SizeOf(...) to initialize this.
      /// </summary>
      public uint dwSize;
      /// <summary>
      /// Flags indicating the valid information returned in this structure. Members that do not contain valid information are set to zero.
      /// </summary>
      public uint dwFlags;
      /// <summary>
      /// Current X-coordinate.
      /// </summary>
      public uint dwXpos;
      /// <summary>
      /// Current Y-coordinate.
      /// </summary>
      public uint dwYpos;
      /// <summary>
      /// Current Z-coordinate.
      /// </summary>
      public uint dwZpos;
      /// <summary>
      /// Current position of the rudder or fourth joystick axis.
      /// </summary>
      public uint dwRpos;
      /// <summary>
      /// Current fifth axis position.
      /// </summary>
      public uint dwUpos;
      /// <summary>
      /// Current sixth axis position.
      /// </summary>
      public uint dwVpos;
      /// <summary>
      /// Current state of the 32 joystick buttons. The value of this member can be set to any combination of JOY_BUTTON n flags, where n is a value in the range of 1 through 32 corresponding to the button that is pressed.
      /// </summary>
      public uint dwButtons;
      /// <summary>
      /// Current button number that is pressed.
      /// </summary>
      public uint dwButtonNumber;
      /// <summary>
      /// Current position of the point-of-view control. Values for this member are in the range 0 through 35,900. These values represent the angle, in degrees, of each view multiplied by 100.
      /// </summary>
      public uint dwPOV;
      /// <summary>
      /// Reserved; do not use.
      /// </summary>
      public uint dwReserved1;
      /// <summary>
      /// Reserved; do not use.
      /// </summary>
      public uint dwReserved2;
    }

    /// <summary>
    /// The JOYCAPS structure contains information about the joystick capabilities.
    /// </summary>
    public struct JoyCaps
    {
      /// <summary>
      /// Manufacturer identifier. Manufacturer identifiers are defined in Manufacturer and Product Identifiers.
      /// </summary>
      public ushort wMid;

      /// <summary>
      /// Product identifier. Product identifiers are defined in Manufacturer and Product Identifiers.
      /// </summary>
      public ushort wPid;

      /// <summary>
      /// Null-terminated string containing the joystick product name.
      /// </summary>
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string szPname;

      /// <summary>
      /// Minimum X-coordinate.
      /// </summary>
      public int wXmin;

      /// <summary>
      /// Maximum X-coordinate.
      /// </summary>
      public int wXmax;

      /// <summary>
      /// Minimum Y-coordinate.
      /// </summary>
      public int wYmin;

      /// <summary>
      /// Maximum Y-coordinate.
      /// </summary>
      public int wYmax;

      /// <summary>
      /// Minimum Z-coordinate.
      /// </summary>
      public int wZmin;

      /// <summary>
      /// Maximum Z-coordinate.
      /// </summary>
      public int wZmax;

      /// <summary>
      /// Number of joystick buttons.
      /// </summary>
      public int wNumButtons;

      /// <summary>
      /// Smallest polling frequency supported when captured by the <see cref="joySetCapture"/> function.
      /// </summary>
      public int wPeriodMin;

      /// <summary>
      /// Largest polling frequency supported when captured by <see cref="joySetCapture"/>.
      /// </summary>
      public int wPeriodMax;

      /// <summary>
      /// Minimum rudder value. The rudder is a fourth axis of movement.
      /// </summary>
      public int wRmin;

      /// <summary>
      /// Maximum rudder value. The rudder is a fourth axis of movement.
      /// </summary>
      public int wRmax;

      /// <summary>
      /// Minimum u-coordinate (fifth axis) values.
      /// </summary>
      public int wUmin;

      /// <summary>
      /// Maximum u-coordinate (fifth axis) values.
      /// </summary>
      public int wUmax;

      /// <summary>
      /// Minimum v-coordinate (sixth axis) values.
      /// </summary>
      public int wVmin;

      /// <summary>
      /// Maximum v-coordinate (sixth axis) values.
      /// </summary>
      public int wVmax;

      /// <summary>
      /// Joystick capabilities The following flags define individual capabilities that a joystick might have:
      /// </summary>
      public int wCaps;

      /// <summary>
      /// Maximum number of axes supported by the joystick.
      /// </summary>
      public int wMaxAxes;

      /// <summary>
      /// Number of axes currently in use by the joystick.
      /// </summary>
      public int wNumAxes;

      /// <summary>
      /// Maximum number of buttons supported by the joystick.
      /// </summary>
      public int wMaxButtons;

      /// <summary>
      /// Null-terminated string containing the registry key for the joystick.
      /// </summary>
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string szRegKey;

      /// <summary>
      /// Null-terminated string identifying the joystick driver OEM.
      /// </summary>
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
      public string szOEMVxD;
    }
  }
}
