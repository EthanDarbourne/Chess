using Assets.Scripts.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {

        public GameObject MainMenuObject;
        public GameObject OptionsMenuObject;

        public Button Chess960Button;

        private bool IsChess960Enabled = false;

        public void Start()
        {
            CustomLogger.CurrentLogLevel = LogLevel.Debug;
            OpenMainMenu();
        }

        public void PlayGame()
        {
            if(IsChess960Enabled)
            {
                SceneManager.LoadSceneAsync(SceneNames.Chess960Scene);
            }
            else 
            {
                SceneManager.LoadSceneAsync(SceneNames.NormalChessScene);
            }
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

        public void ToggleChess960()
        {
            IsChess960Enabled = !IsChess960Enabled; 
            Color32 newColor = IsChess960Enabled
            ? Colors.Green   // green
            : Colors.Red;  // red

            CustomLogger.LogDebug("Changing color");
            Chess960Button.GetComponent<Image>().color = newColor;
        }

        private void CloseAllMenus()
        {
            MainMenuObject.SetActive(false);
            OptionsMenuObject.SetActive(false);
        }
    }
}
