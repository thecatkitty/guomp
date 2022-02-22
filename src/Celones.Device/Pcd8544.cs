using System.Device.Gpio;
using System.Device.Spi;

namespace Celones.Device {
  public class Pcd8544 {
    private SpiDevice _spi;
    private GpioController _gpio;
    private int _res, _dc;

    public int DramSizeX { get => 84; }
    public int DramSizeY { get => 6; }

    public enum PayloadType {
      Command = 0,
      Data = 1
    }

    public enum PowerMode {
      Active = 0,
      PowerDown = 1
    }

    public enum AddressingMode {
      Horizontal = 0,
      Vertical = 1
    }

    public enum InstructionSet {
      Basic = 0,
      Extended = 1
    }

    public enum DisplayMode {
      Blank = 0,
      AllOn = 1,
      Normal = 2,
      Inverse = 3
    }

    public class Instruction {
      public static Instruction NoOperation() => new(0);
      public static Instruction SetOperationMode(PowerMode powerMode = PowerMode.Active, AddressingMode addressingMode = AddressingMode.Horizontal, InstructionSet instructionSet = InstructionSet.Basic) => new((byte)(32 | ((int)powerMode << 2) | ((int)addressingMode << 1) | (int)instructionSet));

      public static Instruction SetDisplayConfiguration(DisplayMode displayMode) => new((byte)(8 | (((int)displayMode & 2) << 1) | ((int)displayMode & 1)));
      public static Instruction SetYAddress(int address) => new((byte)(64 | (address & 7)));
      public static Instruction SetXAddress(int address) => new((byte)(128 | (address & 127)));

      public static Instruction SetTemperatureCoefficient(int coefficient) => new((byte)(4 | (coefficient & 3)));
      public static Instruction SetBiasSystem(int biasSystem) => new((byte)(16 | (biasSystem & 7)));
      public static Instruction SetOperationVoltage(int voltage) => new((byte)(128 | (voltage & 127)));

      public byte Code { get; private set; }

      private Instruction(byte code) {
        Code = code;
      }
    }

    public Pcd8544(SpiDevice spiChannel, GpioController gpioController, int resetPin, int dcPin) {
      _spi = spiChannel;
      _gpio = gpioController;
      _res = resetPin;
      _dc = dcPin;
    }

    public void Initialize() {
      _gpio.OpenPin(_res, PinMode.Output);
      _gpio.OpenPin(_dc, PinMode.Output);

      _gpio.Write(_res, PinValue.Low);
      Thread.Sleep(1);
      _gpio.Write(_res, PinValue.High);

      Write(Instruction.SetOperationMode(instructionSet: InstructionSet.Extended));
      Write(Instruction.SetTemperatureCoefficient(0));
      Write(Instruction.SetBiasSystem(4));
      Write(Instruction.SetOperationMode(instructionSet: InstructionSet.Basic));
      Write(Instruction.SetDisplayConfiguration(DisplayMode.Normal));
    }

    public void Write(byte data)  {
      _gpio.Write(_dc, PinValue.High);
      _spi.Write(stackalloc byte[] {data});
    }

    public void Write(Instruction instruction)  {
      _gpio.Write(_dc, PinValue.Low);
      _spi.Write(stackalloc byte[] {instruction.Code});
    }
  }
}
