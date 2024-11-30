using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sprite = UnityEngine.Sprite;

namespace Assets.VRehab.Scripts
{
    [System.Serializable]
    public class TemplateData
    {
        public Sprite Template;
        public string TemplateName;
        public int Difficulty; // 1 = easy, 2 = medium, 3 = hard
    }
    public class TemplateSpawner : MonoBehaviour
    {
        [Header("UI Elements")]
        public Image Spawner;
        public GameObject DifficultyPromptUi;

        [Header("Template Settings")]
        [SerializeField] private List<TemplateData> _templates = new();
        [SerializeField, Range(1, 3)] private int _selectedDifficulty = 1;

        private readonly List<TemplateData> _usedTemplates = new();
        public SceneFader SceneFade;

        // Start is called before the first frame update
        void Start()
        {
            if (Spawner == null)
            {
                Debug.LogWarning("Spawner Image component is not assigned.");
                return;
            }

            DifficultyPromptUi.SetActive(false);
            SpawnTemplate();
        }

        public void SpawnTemplate()
        {

            if (_templates.Count == 0)
            {
                Debug.LogWarning("Template list is empty.");
                return;
            }
            
            // Get a list of templates filtered by difficulty and usage
            var filteredTemplates = _templates.FindAll(t => 
                t.Difficulty <= _selectedDifficulty && !_usedTemplates.Contains(t));


            if (filteredTemplates.Count == 0)
            {
                switch (_selectedDifficulty)
                {
                    case < 3:
                        DifficultyPromptUi.SetActive(true);
                        break;
                    case 3:
                        Debug.Log("Exercise completed");
                        SceneFade.LoadMainMenu();
                        break;
                }
                return;
            }

            SetRandomTemplate(filteredTemplates);
        }

        private void SetRandomTemplate(List<TemplateData> filteredTemplates)
        {
            // Choose a random template from the filtered list and set it as the current image
            var selectedTemplate = filteredTemplates[Random.Range(0, filteredTemplates.Count)];
            Spawner.sprite = selectedTemplate.Template;

            Debug.Log($"Random template set with difficulty {selectedTemplate.Difficulty}");

            _usedTemplates.Add(selectedTemplate); // Mark template as used
        }

        // Called by gestureRecognizer after a gesture is completed
        public void SpawnNewTemplate()
        {
            StartCoroutine(WaitAndSetNextImage());
        }

        private IEnumerator WaitAndSetNextImage()
        {
            yield return new WaitForSeconds(3);
            SpawnTemplate();
        }

        // Called by difficultyPromptUI when user opts to increase difficulty
        public void IncreaseDifficulty()
        {
            _selectedDifficulty++;
            DifficultyPromptUi.SetActive(false);
            SpawnTemplate();
        }

        public string GetTemplateName()
        {
            return _usedTemplates[^1].TemplateName; 
        }
    }
}