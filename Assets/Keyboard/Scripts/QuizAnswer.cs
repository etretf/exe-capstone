using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class QuizOptionButton : MonoBehaviour
{
    public string questionId = "q1";
    public TMP_Text label;

    void Awake()
    {
        if (label == null) label = GetComponentInChildren<TMP_Text>();
        GetComponent<Button>().onClick.AddListener(Click);
    }

    void Click()
    {
        if (label == null) { Debug.LogError("the button label was not found."); return; }

        QuizManager.Instance.SetAnswer(questionId, label.text);
        Debug.Log($"[Quiz] Selected: {questionId} = {label.text}");
    }
}

