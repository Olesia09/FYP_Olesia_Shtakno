using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.VRehab.Scripts
{
    public class SceneFader : MonoBehaviour
    {
        public Animator Transition;
        public float TransitionTime = 1f;
        public static SceneFader Instance;

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadLevel(sceneName));
        }

        // Specific load methods using scene names
        public void LoadMainMenu()
        {
            LoadScene("Menu");
        }

        public void LoadSorting()
        {
            LoadScene("Sorting");
        }

        public void LoadClimbing()
        {
            LoadScene("Climbing");
        }

        public void LoadPainting()
        {
            LoadScene("Painting");
        }

        public void LoadSlicing()
        {
            LoadScene("Slicing");
        }

        // Coroutine to handle loading by scene name
        private IEnumerator LoadLevel(string sceneName)
        {
            if (!Transition.gameObject.activeInHierarchy)
            {
                Transition.gameObject.SetActive(true);
            }

            Transition.SetTrigger("End");
            yield return new WaitForSeconds(TransitionTime);
            SceneManager.LoadScene(sceneName);
            Transition.SetTrigger("Start");
        }
    }
}