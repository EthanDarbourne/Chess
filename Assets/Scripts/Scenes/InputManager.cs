using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenes
{
    public class InputManager
    {
        public bool CheckForKeyCodeDown(KeyCode code)
        {
            return Input.GetKeyDown(code);
        }

        public bool CheckForKeyCodeUp(KeyCode code)
        {
            return Input.GetKeyUp(code);
        }

        public bool CheckForKeyCodeHeld(KeyCode code)
        {
            return Input.GetKey(code);
        }

        public bool CheckForLeftMouseDown()
        {
            return Input.GetMouseButtonDown(MouseInputs.LeftMouse);
        }

        public bool CheckForRightMouseDown()
        {
            return Input.GetMouseButtonDown(MouseInputs.RightMouse);
        }
        public bool CheckForMiddleMouseDown()
        {
            return Input.GetMouseButtonDown(MouseInputs.MiddleMouse);
        }

        public List<PlayerInputs> CheckForPlayerInputs()
        {
            List<PlayerInputs> inputs = new();

            return inputs;

        }
    }
}
