using Assets.VRehab.Scripts;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DisplayGameStatistics : MonoBehaviour
{
    public GameObject ScrollContent; 
    public GameObject TextPrefab;

    private string slicingScoresFilePath;
    private string paintingScoresFilePath;


    private void Start()
    {
       
        slicingScoresFilePath = Path.Combine(Application.persistentDataPath, "slicing_scores.txt");
        paintingScoresFilePath = Path.Combine(Application.persistentDataPath, "painting_scores.txt");
    }
    public void LoadSlicingScores()
    {
        var slicingScores = LoadScoresFromFile(slicingScoresFilePath);
        DisplayScores(slicingScores);
    }

    public void LoadPaintingScores()
    {
        var paintingScores = LoadScoresFromFile(paintingScoresFilePath);
        DisplayScores(paintingScores);
    }

    private List<string> LoadScoresFromFile(string filePath)
    {
        var scores = new List<string>();

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new (filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    scores.Add(line);
                }
            }
        }
        else
        {
            Debug.LogWarning($"Scores file not found: {filePath}");
        }

        return scores;
    }

    private void DisplayScores(List<string> scores)
    {
        // refresh entries
        foreach (Transform child in ScrollContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate new entries
        foreach (var score in scores)
        {
            var textObj = Instantiate(TextPrefab, ScrollContent.transform);
            var textComponent = textObj.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = score;
            }
        }
    }
}

