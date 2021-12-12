using UnityEngine;

namespace Game.View
{
    public enum MouseType
    {
        Nagative = -1,
        None = 0,
        Positive = 1,
    }
    public class InputSystem
    {
        public const int skillCount = 20;
        public bool[] skillBtns = new bool[skillCount];
        public bool[] skillBtnings = new bool[skillCount];
        public MouseType xCurInput = MouseType.None, yCurInput = MouseType.None;

        public InputInteration ad = new InputInteration(KeyCode.A,KeyCode.D);
        public InputInteration sw = new InputInteration(KeyCode.S,KeyCode.W);


        public KeyCode[] KeyCodes =
        {
            KeyCode.Space,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.U,
            KeyCode.I,
            KeyCode.O,
            KeyCode.P,
        };
        
        public KeyCode[] KeyCodeBs =
        {
            KeyCode.Space,
            KeyCode.Keypad1,
            KeyCode.Keypad2,
            KeyCode.Keypad3,
            KeyCode.Keypad4,
            KeyCode.Keypad5,
            KeyCode.Keypad6,
            KeyCode.Keypad7,
            KeyCode.Keypad8,
            KeyCode.Keypad9,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.U,
            KeyCode.I,
            KeyCode.O,
            KeyCode.P,
        };
        
        public void UpdateInput()
        {
            bool op = false;
            var adcode = ad.GetCurCode(out op);
            var wscode = sw.GetCurCode(out op);
            xCurInput = ConvertKeyCode2MouseType(ad,adcode);
            yCurInput = ConvertKeyCode2MouseType(sw,wscode);

            for (int i = 0; i < KeyCodes.Length; i++)
            {
                skillBtns[i] = Input.GetKeyDown(KeyCodes[i]) || Input.GetKeyDown(KeyCodeBs[i]);
                skillBtnings[i] = Input.GetKey(KeyCodes[i]) || Input.GetKey(KeyCodeBs[i]);
            }

        }

        private MouseType ConvertKeyCode2MouseType(InputInteration interation,KeyCode adcode)
        {
            if (adcode == interation.x)
            {
                return MouseType.Nagative;
            }else if (adcode == interation.y)
            {
                return MouseType.Positive;
            }
            else
            {
                return MouseType.None;
            }
        }
    }
}