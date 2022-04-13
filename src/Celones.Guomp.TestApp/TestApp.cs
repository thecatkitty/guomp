using Celones.Guomp;
using Celones.Guomp.Displays;
using Celones.Guomp.TestApp;

var spi = NullSpiDevice.Create();
var gpio = NullGpioController.Create();
var pwm = NullPwmChannel.Create();

var pcd = new Celones.Device.Pcd8544(spi, gpio, 0, 0);
var display = new Nokia5110(pcd, pwm);
var ui = new Ui("TestApp.xaml", display);

ui.Show();

if (!Directory.Exists("capture"))
    Directory.CreateDirectory("capture");

display.Capture(string.Format("capture/{0:yyyy}{0:MM}{0:dd}_{0:HH}{0:mm}{0:ss}.{0:fff}.png", DateTime.Now));
