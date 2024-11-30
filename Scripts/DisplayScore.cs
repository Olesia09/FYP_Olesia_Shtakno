using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace Assets.VRehab.Scripts
{
    public class DisplayScore : MonoBehaviour
    {
        public GameObject ScorePanel;
        public TMP_Text ScoreText;

        private string _filePath;

        private void Awake()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "painting_scores.txt");
        }

        public void Start()
        {
            if (ScorePanel == null) return;
            ScorePanel.SetActive(false);
            Debug.Log("ScorePanel hidden.");
        }

        public void UpdateScore(float score, string shape)
        {
            if (ScorePanel == null) return;
            ScorePanel.SetActive(true);
            Debug.Log("ScorePanel active");

            var scorePercentage = Mathf.Round(score * 1000f) / 10f;
            string newScore = $"{shape}: {scorePercentage:F1}%";
            ScoreText.text = newScore;

            SaveScore(newScore);
            Invoke(nameof(HideScorePanel), 3f);
            Debug.Log("Hiding panel");
        }

        private void SaveScore(string newScore)
        {
            // Load existing data
            List<string> scores = LoadScores();
            scores.Add(newScore);

            // Save updated data
            File.WriteAllText(_filePath, JsonUtility.ToJson(new ScoreList { Scores = scores }));
        }

        public List<string> LoadScores()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                ScoreList scoreList = JsonUtility.FromJson<ScoreList>(json);
                return scoreList.Scores;
            }
            return new List<string>();
        }

        [System.Serializable]
        public class ScoreList
        {
            public List<string> Scores;
        }

        public void InaccurateDrawing()
        {
            if (ScorePanel == null) return;
            ScorePanel.SetActive(true);
            Debug.Log("ScorePanel active");

            ScoreText.text = "Inaccurate shape recognized";

            Invoke(nameof(HideScorePanel), 3f);
            Debug.Log("Hiding panel");
        }

        private void HideScorePanel()
        {
            if (ScorePanel != null)
                ScorePanel.SetActive(false);
        }
    }
}
