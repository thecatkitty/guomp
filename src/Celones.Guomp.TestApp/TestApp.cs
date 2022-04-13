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

display.Capture("capture.png");
