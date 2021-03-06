using System;
using System.Runtime.InteropServices;

namespace LibPlasticInstrument
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
    public struct Gamepad
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
    public struct GamepadEx
    {
      public Buttons wButtons;
      public byte bLeftTrigger;
      public byte bRightTrigger;
      public short sThumbLX;
      public short sThumbLY;
      public short sThumbRX;
      public short sThumbRY;
      public uint dwUnknown;
      public void Print()
      {
        Console.WriteLine($"wButtons      {wButtons}");
        Console.WriteLine($"bLeftTrigger  {bLeftTrigger:X2}");
        Console.WriteLine($"bRightTrigger {bRightTrigger:X2}");
        Console.WriteLine($"sThumbLX      {sThumbLX:X4}");
        Console.WriteLine($"sThumbLY      {sThumbLY:X4}");
        Console.WriteLine($"sThumbRX      {sThumbRX:X4}");
        Console.WriteLine($"sThumbRY      {sThumbRY:X4}");
        Console.WriteLine($"dwUnknown     {dwUnknown:X8}");
      }
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct State
    {
      public uint dwPacketNumber;
      public Gamepad Gamepad;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct StateEx
    {
      public uint dwPacketNumber;
      public GamepadEx Gamepad;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct Vibration
    {
      public ushort wLeftMotorSpeed;
      public ushort wRightMotorSpeed;
    };

    public enum DevSubType : byte
    {
      Unknown = 0,
      Gamepad = 1,
      Wheel = 2,
      ArcadeStick = 3,
      FlightStick = 4,
      DancePad = 5,
      Guitar = 6,
      GuitarAlternate = 7,
      DrumKit = 8,
      UNK_9 = 9,
      UNK_10 = 10,
      GuitarBass = 11,
      UNK_12 = 12,
      UNK_13 = 13,
      UNK_14 = 14,
      Keytar = 15,
      UNK_16 = 16, 
      UNK_17 = 17,
      UNK_18 = 18,
      ArcadePad = 19,
      ProGuitar = 25
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Capabilities
    {
      public byte Type;
      public DevSubType SubType;
      public ushort Flags;
      public Gamepad Gamepad;
      public Vibration Vibration;
    };

    public struct BatteryInformation
    {
      public BatteryType BatteryType;
      public BatteryLevel BatteryLevel;
    };

    public enum BatteryDevType : byte
    {
      Gamepad = 0,
      Headset = 1,
    }
    public enum BatteryType : byte
    {
      Disconnected = 0,
      Wired = 1,
      Alkaline = 2,
      NiMH = 3,
      Unknown = 0xFF
    }
    public enum BatteryLevel : byte
    {
      Empty = 0,
      Low = 1,
      Medium = 2,
      Full = 3,
    }
    [DllImport("xinput1_4")]
    public static extern uint XInputGetState(uint dwUserIndex, ref State pState);
    [DllImport("xinput1_4", EntryPoint = "#100")]
    public static extern uint XInputGetStateEx(uint dwUserIndex, ref StateEx pState);
    [DllImport("xinput1_4")]
    public static extern uint XInputGetCapabilities(
      uint dwUserIndex,   // Index of the gamer associated with the device
      uint dwFlags,       // Input flags that identify the device type
      ref Capabilities pCapabilities  // Receives the capabilities
    );
    [DllImport("xinput1_4")]
    public static extern uint XInputGetBatteryInformation(
      uint dwUserIndex,  // Index of the gamer associated with the device
      BatteryDevType devType,      // A BATTERY_DEVTYPE
      ref BatteryInformation pBatteryInformation // Battery status
    );
  }
}
