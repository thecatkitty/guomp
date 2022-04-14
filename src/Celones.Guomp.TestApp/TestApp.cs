using Celones.Guomp;
using Celones.Guomp.Controls;
using Celones.Guomp.Displays;
using Celones.Guomp.TestApp;

var spi = NullSpiDevice.Create();
var gpio = NullGpioController.Create();
var pwm = NullPwmChannel.Create();

var pcd = new Celones.Device.Pcd8544(spi, gpio, 0, 0);
var display = new Nokia5110(pcd, pwm);
var ui = new Ui("TestApp.xaml", display);

if (!Directory.Exists("capture"))
    Directory.CreateDirectory("capture");

while (true)
{
    ui.Show();
    display.Capture(string.Format("capture/{0:yyyy}{0:MM}{0:dd}_{0:HH}{0:mm}{0:ss}.{0:fff}.png", DateTime.Now));

    var menu = ui.Root.Items.OfType<Menu>().First();
    var index = menu.Items.IndexOf(menu.SelectedItem);

    // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.Escape:
            return;

        case ConsoleKey.UpArrow:
            if (index != 0)
                menu.SelectedItem = (MenuItem)menu.Items[index - 1];
            break;

        case ConsoleKey.DownArrow:
            if (index != (menu.Items.Count - 1))
                menu.SelectedItem = (MenuItem)menu.Items[index + 1];
            break;
    }
}
