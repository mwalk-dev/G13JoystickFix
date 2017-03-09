using System.ComponentModel;
using System.Windows;

namespace G13JoystickFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        SettingsViewModel vm;
        private bool exiting = false;

        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = vm = new SettingsViewModel();
            vm.Load();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!exiting)
            {
                e.Cancel = true;
                WindowState = WindowState.Minimized;
                NotifyIcon.ShowBalloonTip("Settings hidden", string.Format("{0} is still running. Right-click the system tray icon and select 'Exit' to end the program.", Application.Current.MainWindow.GetType().Assembly.GetName().Name), Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            exiting = true;
            Application.Current.Shutdown();               
        }
    }
}
