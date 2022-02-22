using System.Device.Gpio;
using System.Device.Spi;

namespace Celones.Device {
  public class Pcd8544 {
    private readonly SpiDevice m_spi;
    private readonly GpioController m_gpio;
    private readonly int m_res;
    private readonly int m_dc;

    public int DramSizeX => 84;
    public int DramSizeY => 6;

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

      public byte Code { get; }

      private Instruction(byte code) {
        Code = code;
      }
    }

    public Pcd8544(SpiDevice spiChannel, GpioController gpioController, int resetPin, int dcPin) {
      m_spi = spiChannel;
      m_gpio = gpioController;
      m_res = resetPin;
      m_dc = dcPin;
    }

    public void Initialize() {
      m_gpio.OpenPin(m_res, PinMode.Output);
      m_gpio.OpenPin(m_dc, PinMode.Output);

      m_gpio.Write(m_res, PinValue.Low);
      Thread.Sleep(1);
      m_gpio.Write(m_res, PinValue.High);

      Write(Instruction.SetOperationMode(instructionSet: InstructionSet.Extended));
      Write(Instruction.SetTemperatureCoefficient(0));
      Write(Instruction.SetBiasSystem(4));
      Write(Instruction.SetOperationMode(instructionSet: InstructionSet.Basic));
      Write(Instruction.SetDisplayConfiguration(DisplayMode.Normal));
    }

    public void Write(byte data)  {
      m_gpio.Write(m_dc, PinValue.High);
      m_spi.Write(stackalloc byte[] {data});
    }

    public void Write(Instruction instruction)  {
      m_gpio.Write(m_dc, PinValue.Low);
      m_spi.Write(stackalloc byte[] {instruction.Code});
    }
  }
}
