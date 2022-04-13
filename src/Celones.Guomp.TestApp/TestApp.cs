using Celones.Guomp;
using Celones.Guomp.Displays;

var pcd = new Celones.Device.Pcd8544(null, null, 0, 0);
var display = new Nokia5110(pcd, null);
var ui = new Ui("TestApp.xaml", display);

ui.Show();

display.Capture("capture.png");
