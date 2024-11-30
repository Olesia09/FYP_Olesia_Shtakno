using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.VRehab.Scripts
{
    public class PauseManager : MonoBehaviour
    {
        public GameObject PauseMenuUi;
        public GameObject OptionsMenuUi;
        public GameObject StatisticsMenuUi;
        private bool _isPaused = false;

        public InputActionReference PauseAction;


        public void Awake()
        {
            PauseMenuUi.SetActive(false);
        }

        private void OnEnable()
        {
            PauseAction.action.started += OnPauseAction; 
        }

        private void OnDisable()
        {
            PauseAction.action.started -= OnPauseAction; 
        }

        private void OnPauseAction(InputAction.CallbackContext context)
        {
            TogglePause();
        }

        private void TogglePause()
        {
            _isPaused = !_isPaused;
            PauseMenuUi.SetActive(_isPaused);
            OptionsMenuUi.SetActive(_isPaused);
            StatisticsMenuUi.SetActive(_isPaused);

            Time.timeScale = _isPaused ? 0f : // Freeze the game
                1f; // Resume the game
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1f; // Reset time scale
            SceneFader.Instance.LoadMainMenu(); // Use your Scene Fader to go to the main menu
        }
    }
}