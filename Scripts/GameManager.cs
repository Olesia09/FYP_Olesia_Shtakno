using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace Assets.VRehab.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public TMP_Text HitText;
        public TMP_Text MissedText;
        public GameObject ResultsPanel;
        public AudioSource AudioSource;
        public int BPM;
        public SceneFader SceneFader;

        private int _hitCount;
        private int _missedCount;
        private int _activeCubes;
        private float _spawnDistance = 20.5f;
        private float _cubeSpeed = 2f;
        protected float Delay;
        private string _filePath;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            Delay = _spawnDistance / _cubeSpeed;

            _filePath = Path.Combine(Application.persistentDataPath, "slicing_scores.txt");
        }

        private void Start()
        {
            ResultsPanel.SetActive(false);
            StartSongWithDelay();
        }

        public void StartSongWithDelay()
        {
            Invoke(nameof(StartSong), Delay); // Delay song start to sync with first cube
        }

        private void StartSong()
        {
            AudioSource.Play();
        }

        public void IncrementHit()
        {
            _hitCount++;
            _activeCubes--;
            UpdateUI();
        }

        public void IncrementMissed()
        {
            _missedCount++;
            _activeCubes--;
            UpdateUI();
        }

        public void IncrementActiveCubes()
        {
            _activeCubes++;
        }

        private void UpdateUI()
        {
            HitText.text = "Hit: " + _hitCount;
            MissedText.text = "Missed: " + _missedCount;

            if (_activeCubes <= 0 && !AudioSource.isPlaying)
            {
                SaveResults();
                StartCoroutine(ShowResultsAfterDelay());
            }
        }

        private IEnumerator ShowResultsAfterDelay()
        {
            yield return new WaitForSeconds(2f); // Optional delay before showing results
            ResultsPanel.SetActive(true);
            yield return new WaitForSeconds(5f);
            SceneFader.LoadMainMenu();
        }

        public float GetDelay()
        {
            return Delay;
        }

        private void SaveResults()
        {
            // Load existing data
            List<string> scores = LoadScores();
            string newScore = $"Hit: {_hitCount}, Missed: {_missedCount}";
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
    }
}
