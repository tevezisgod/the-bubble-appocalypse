using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Controllers
{
    //this class is a temporary measure to allow navigation between scenes (The start and end menus were added last)
    //the game controls should, of course, be initialized once
    public class MenuHelper:  MonoBehaviour
    {
        private Controls GameControls;

        private void OnEnable()
        {
            GameControls = new Controls();
            GameControls.Enable();
            GameControls.Player1.Quit.performed += PlayerQuit;
        }

        private void PlayerQuit(InputAction.CallbackContext obj)
        {
            Application.Quit();
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void OnDisable()
        {
            GameControls.Player1.Quit.performed -= PlayerQuit;
        }
    }
}
