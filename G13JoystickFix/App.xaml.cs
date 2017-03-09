using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace G13JoystickFix
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        SettingsWindow window;
        SettingsViewModel vm;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += (s, args) =>
            {
                if(args.ExceptionObject is Exception)
                {
                    MessageBox.Show(((Exception)args.ExceptionObject).StackTrace, "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            };

            window = new SettingsWindow();
            vm = (SettingsViewModel)window.DataContext;

            var reader = new ThumbstickReader();
            try
            {
                reader.Start();
            }
            catch(G13NotFoundException)
            {
                MessageBox.Show("Unable to locate a Logitech G13. Please ensure that it is connected.", "Hardware Detection Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            var translator = new InputTranslator(vm);
            var sender = new InputSender();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    sender.SendKeys(translator.Translate(reader.GetInputs()));

                    Thread.Sleep(1);
                }
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            window.NotifyIcon.Dispose();
            vm.Save();
        }
    }
}
