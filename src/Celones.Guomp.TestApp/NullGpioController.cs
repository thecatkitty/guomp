using System.Device.Gpio;
using System.Runtime.Serialization;

namespace Celones.Guomp.TestApp
{
    internal class NullGpioController : GpioController
    {
        public override PinValue Read(int pinNumber) => PinValue.High;

        public override void Write(int pinNumber, PinValue value)
        {
        }

        public static NullGpioController Create() =>
            (NullGpioController)FormatterServices.GetUninitializedObject(typeof(NullGpioController));
    }
}
