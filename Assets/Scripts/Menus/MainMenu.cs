using Assets.Scripts.GameObjects;
using Assets.Scripts.Misc;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject MainMenuObject;
        public GameObject OptionsMenuObject;

        public List<Button> Buttons;

        private ButtonManager _buttonManager;

        public void Start()
        {
            CustomLogger.CurrentLogLevel = LogLevel.Debug;
            OpenMainMenu();
            _buttonManager = new(Buttons);
            _buttonManager.ToggleButton(Buttons[0]);
        }

        public void PlayGame()
        {
            string selectedButton = _buttonManager.GetSelectedButtonName;
            SceneManager.LoadSceneAsync(selectedButton);
        }

        public void Quit()
        {
            CustomLogger.LogDebug("Quitting");
            Application.Quit();
        }

        public void OpenMainMenu()
        {
            CloseAllMenus();
            MainMenuObject.SetActive(true);
        }

        public void OpenOptionsMenu()
        {
            CloseAllMenus();
            OptionsMenuObject.SetActive(true);
        }

        public void ToggleButton(Button button)
        {
            _buttonManager.ToggleButton(button);
        }

        private void CloseAllMenus()
        {
            MainMenuObject.SetActive(false);
            OptionsMenuObject.SetActive(false);
        }
    }
}
