using SharpDX.DirectInput;
using System;

namespace G13JoystickFix
{
    class ThumbstickReader
    {
        private readonly string G13_GUID = "c2ab046d-0000-0000-0000-504944564944";
        private DirectInput directInput = new DirectInput();
        private Joystick thumbstick;
        private bool started = false;

        public void Start()
        {
            if (started)
                return;
            thumbstick = GetThumbstick();
            if (thumbstick != null)
            {
                thumbstick.Properties.BufferSize = 128;
                thumbstick.Acquire();
            }
            else
                throw new G13NotFoundException();
            started = true;
        }

        public void Stop()
        {
            if (!started)
                return;
            thumbstick.Unacquire();
            started = false;
        }

        public JoystickState GetInputs()
        {
            thumbstick.Poll();
            return thumbstick.GetCurrentState();
        }

        private Joystick GetThumbstick()
        {
            // Find a Joystick Guid
            var joystickGuid = Guid.Empty;
            foreach (var deviceInstance in directInput.GetDevices())
            {
                if (deviceInstance.ProductGuid.ToString() == G13_GUID)
                {
                    return new Joystick(directInput, deviceInstance.InstanceGuid);
                }
            }
            return null;
        }
    }


}
