using SharpDX.DirectInput;
using System;
using System.ComponentModel;

namespace G13JoystickFix
{
    class InputTranslator : IDisposable
    {
        private SettingsViewModel vm;
        private readonly int AXIS_MAX = 65535;
        private Tuple<int, int> deadzoneXRange;
        private Tuple<int, int> deadzoneYRange;

        public InputTranslator(SettingsViewModel vm)
        {
            this.vm = vm;
            vm.PropertyChanged += vm_PropertyChanged;
            SetDeadzones();
        }

        public void Dispose()
        {
            vm.PropertyChanged -= vm_PropertyChanged;
        }

        private void vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "XAxisDeadzone" || e.PropertyName == "YAxisDeadzone")
                SetDeadzones();
        }

        private void SetDeadzones()
        {
            deadzoneXRange = new Tuple<int, int>(AXIS_MAX / 2 - vm.XAxisDeadzone, AXIS_MAX / 2 + vm.XAxisDeadzone);
            deadzoneYRange = new Tuple<int, int>(AXIS_MAX / 2 - vm.YAxisDeadzone, AXIS_MAX / 2 + vm.YAxisDeadzone);
        }

        private JoystickState oldInput;
        private KeyState state = new KeyState();
        public KeyData Translate(JoystickState input)
        {
            var keyData = new KeyData();

            var restrictX = vm.FourWayInput && Math.Abs(AXIS_MAX / 2 - input.X) < Math.Abs(AXIS_MAX / 2 - input.Y);
            var restrictY = vm.FourWayInput && Math.Abs(AXIS_MAX / 2 - input.X) >= Math.Abs(AXIS_MAX / 2 - input.Y);

            // Press left key
            if (input.X < deadzoneXRange.Item1 && !restrictX && !state.Left)
            {
                state.Left = true;
                keyData.KeysDown.Add(vm.LeftInput);
            }
            // Release left key
            else if((input.X >= deadzoneXRange.Item1 || restrictX) && state.Left)
            {
                state.Left = false;
                keyData.KeysUp.Add(vm.LeftInput);
            }

            // Press right key
            if(input.X > deadzoneXRange.Item2 && !restrictX && !state.Right)
            {
                state.Right = true;
                keyData.KeysDown.Add(vm.RightInput);
            }
            // Release right key
            else if ((input.X <= deadzoneXRange.Item2 || restrictX) && state.Right)
            {
                state.Right = false;
                keyData.KeysUp.Add(vm.RightInput);
            }

            // Press up key
            if (input.Y < deadzoneYRange.Item1 && !restrictY && !state.Up)
            {
                state.Up = true;
                keyData.KeysDown.Add(vm.UpInput);
            }
            // Release up key
            else if ((input.Y >= deadzoneYRange.Item1 || restrictY) && state.Up)
            {
                state.Up = false;
                keyData.KeysUp.Add(vm.UpInput);
            }

            // Press down key
            if (input.Y > deadzoneYRange.Item2 && !restrictY && !state.Down)
            {
                state.Down = true;
                keyData.KeysDown.Add(vm.DownInput);
            }
            // Release down key
            else if ((input.Y <= deadzoneYRange.Item2 || restrictY) && state.Down)
            {
                state.Down = false;
                keyData.KeysUp.Add(vm.DownInput);
            }
            oldInput = input;

            return keyData;
        }

        private class KeyState
        {
            public bool Left { get; set; }
            public bool Right { get; set; }
            public bool Up { get; set; }
            public bool Down { get; set; }
        }
    }
}
