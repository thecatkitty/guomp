using System.Device.Pwm;

namespace Celones.Guomp.TestApp
{
    internal class NullPwmChannel : PwmChannel
    {
        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        public override int Frequency { get; set; }
        public override double DutyCycle { get; set; }

        public static NullPwmChannel Create() => new();
    }
}
