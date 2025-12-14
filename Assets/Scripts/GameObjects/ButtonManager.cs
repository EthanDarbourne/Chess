using Assets.Scripts.Misc;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Assets.Scripts.GameObjects
{
    public class ButtonManager
    {
        private int _currentButtonIndex = 0;

        private readonly List<Button> _gameButtons = new();
        private readonly List<string> _gameButtonNames = new();

        public string GetSelectedButtonName => _gameButtonNames[_currentButtonIndex];

        public ButtonManager(){}

        public ButtonManager(List<Button> buttons)
        {
            _gameButtons = buttons;
            _gameButtonNames = buttons.Select(b => b.name).ToList();
        }

        public void RegisterButton(string buttonName, Button button)
        {
            _gameButtons.Add(button);
            _gameButtonNames.Add(buttonName);
        }

        public void ToggleButton(Button button)
        {
            int index = _gameButtons.IndexOf(button);
            if(index == -1)
            {
                CustomLogger.LogError($"Tried to toggle button that does not exist: {button.name}");
                throw new System.Exception($"Missing button: {button.name}");
            }
            _gameButtons[_currentButtonIndex].GetComponent<Image>().color = Colors.Red;

            _gameButtons[index].GetComponent<Image>().color = Colors.Green;
            _currentButtonIndex = index;
        }

    }
}
