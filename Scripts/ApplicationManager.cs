using UnityEngine;

namespace Assets.VRehab.Scripts
{
    public class ApplicationManager : MonoBehaviour
    {
        public void QuitApplication()
        {
            // If running in a standalone build
            Application.Quit();

            // If in the editor
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

        }
    }
}
