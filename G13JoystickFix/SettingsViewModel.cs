using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace G13JoystickFix
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int xAxisDeadzone;
        public int XAxisDeadzone
        {
            get { return xAxisDeadzone; }
            set { SetField(ref xAxisDeadzone, value); }
        }

        private int yAxisDeadzone;
        public int YAxisDeadzone
        {
            get { return yAxisDeadzone; }
            set { SetField(ref yAxisDeadzone, value); }
        }

        private char upInput;
        public char UpInput
        {
            get { return upInput; }
            set { SetField(ref upInput, value); }
        }

        private char leftInput;
        public char LeftInput
        {
            get { return leftInput; }
            set { SetField(ref leftInput, value); }
        }

        private char downInput;
        public char DownInput
        {
            get { return downInput; }
            set { SetField(ref downInput, value); }
        }

        private char rightInput;
        public char RightInput
        {
            get { return rightInput; }
            set { SetField(ref rightInput, value); }
        }

        private bool fourWayInput;
        public bool FourWayInput
        {
            get { return fourWayInput; }
            set { SetField(ref fourWayInput, value); }
        }

        public SettingsViewModel()
        {

        }

        public void Load()
        {
            XAxisDeadzone = Properties.Settings.Default.XAxisDeadzone;
            YAxisDeadzone = Properties.Settings.Default.YAxisDeadzone;

            UpInput = Properties.Settings.Default.UpInput;
            LeftInput = Properties.Settings.Default.LeftInput;
            DownInput = Properties.Settings.Default.DownInput;
            RightInput = Properties.Settings.Default.RightInput;

            FourWayInput = Properties.Settings.Default.FourWayInput;
        }

        public void Save()
        {
            Properties.Settings.Default.XAxisDeadzone = XAxisDeadzone;
            Properties.Settings.Default.YAxisDeadzone = YAxisDeadzone;

            Properties.Settings.Default.UpInput = UpInput;
            Properties.Settings.Default.LeftInput = LeftInput;
            Properties.Settings.Default.DownInput = DownInput;
            Properties.Settings.Default.RightInput = RightInput;

            Properties.Settings.Default.FourWayInput = FourWayInput;

            Properties.Settings.Default.Save();
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
