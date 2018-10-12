using System;
using System.Runtime.InteropServices;

namespace KeytarPoller
{
  public static class XInput
  {
    public const uint XUSER_MAX_COUNT = 4;

    [Flags]
    public enum Buttons : ushort
    {
      DPAD_UP        = 0x0001,
      DPAD_DOWN      = 0x0002,
      DPAD_LEFT      = 0x0004,
      DPAD_RIGHT     = 0x0008,
      START          = 0x0010,
      BACK           = 0x0020,
      LEFT_THUMB     = 0x0040,
      RIGHT_THUMB    = 0x0080,
      LEFT_SHOULDER  = 0x0100,
      RIGHT_SHOULDER = 0x0200,
      A              = 0x1000,
      B              = 0x2000,
      X              = 0x4000,
      Y              = 0x8000,
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_GAMEPAD
    {
      public Buttons wButtons;
      public byte bLeftTrigger;
      public byte bRightTrigger;
      public short sThumbLX;
      public short sThumbLY;
      public short sThumbRX;
      public short sThumbRY;
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_GAMEPAD_EX
    {
      public Buttons wButtons;
      public byte bLeftTrigger;
      public byte bRightTrigger;
      public short sThumbLX;
      public short sThumbLY;
      public short sThumbRX;
      public short sThumbRY;
      public uint dwUnknown;
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_STATE
    {
      public uint dwPacketNumber;
      public XINPUT_GAMEPAD Gamepad;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_STATE_EX
    {
      public uint dwPacketNumber;
      public XINPUT_GAMEPAD_EX Gamepad;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_VIBRATION
    {
      public ushort wLeftMotorSpeed;
      public ushort wRightMotorSpeed;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_CAPABILITIES
    {
      public byte Type;
      public byte SubType;
      public ushort Flags;
      public XINPUT_GAMEPAD Gamepad;
      public XINPUT_VIBRATION Vibration;
    };

    public struct XINPUT_BATTERY_INFORMATION
    {
      public byte BatteryType;
      public byte BatteryLevel;
    };

    [DllImport("xinput1_4")]
    public static extern uint XInputGetState(uint dwUserIndex, ref XINPUT_STATE pState);
    [DllImport("xinput1_4", EntryPoint = "#100")]
    public static extern uint XInputGetStateEx(uint dwUserIndex, ref XINPUT_STATE_EX pState);
    [DllImport("xinput1_4")]
    public static extern uint XInputGetCapabilities(
      uint dwUserIndex,   // Index of the gamer associated with the device
      uint dwFlags,       // Input flags that identify the device type
      ref XINPUT_CAPABILITIES pCapabilities  // Receives the capabilities
    );
  }
}
