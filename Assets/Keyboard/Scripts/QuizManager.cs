using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance { get; private set; }

    [Header("UI")]
    public TMP_Text debugStatusText;  
    public string saveKeyPrefix = "QUIZ_"; 

    private Dictionary<string, string> answers = new Dictionary<string, string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public void SetAnswer(string questionId, string answerValue)
    {
        if (string.IsNullOrWhiteSpace(questionId)) return;

        answers[questionId] = answerValue;

        if (debugStatusText != null)
            debugStatusText.text = $"Saved: {questionId} = {answerValue}";
    }

    public string GetAnswer(string questionId, string defaultValue = "")
    {
        if (answers.TryGetValue(questionId, out var v)) return v;

        string key = saveKeyPrefix + questionId;
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key);

        return defaultValue;
    }

    public void SubmitAndSave()
    {
        foreach (var kv in answers)
        {
            PlayerPrefs.SetString(saveKeyPrefix + kv.Key, kv.Value);
        }
        PlayerPrefs.Save();

        Debug.Log($"[Quiz] Submitted. Saved {answers.Count} answers.");
        Debug.Log($"[Quiz] q1 = {GetAnswer("q1", "NOT SET")}");

        if (debugStatusText != null)
            debugStatusText.text = "Quiz submitted + saved!";
    }

    public void ClearAll()
    {
        foreach (var kv in answers)
            PlayerPrefs.DeleteKey(saveKeyPrefix + kv.Key);

        answers.Clear();
        PlayerPrefs.Save();
    }
}
