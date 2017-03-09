using System.Collections.Generic;

namespace G13JoystickFix
{
    class KeyData
    {
        public List<char> KeysUp { get; private set; }
        public List<char> KeysDown { get; private set; }

        public KeyData()
        {
            KeysUp = new List<char>();
            KeysDown = new List<char>();
        }
    }
}
