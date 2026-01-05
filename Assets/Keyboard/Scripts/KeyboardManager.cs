using UnityEngine;
using TMPro;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager Instance { get; private set; }

    [Header("Optional default field (if you want)")]
    public TMP_InputField defaultField;

    public TMP_InputField ActiveField { get; private set; }

    void Awake()
    {
        // Simple singleton so keys + fields can talk to one keyboard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (defaultField != null)
            SetActiveField(defaultField);
    }

    public void SetActiveField(TMP_InputField field)
    {
        ActiveField = field;
        if (ActiveField != null)
            ActiveField.ActivateInputField();
    }

    public void Type(string text)
    {
        if (ActiveField == null) return;

        ActiveField.text += text;
        ActiveField.caretPosition = ActiveField.text.Length;
        ActiveField.ActivateInputField();
    }

    public void Backspace()
    {
        if (ActiveField == null) return;

        string t = ActiveField.text;
        if (t.Length == 0) return;

        ActiveField.text = t.Substring(0, t.Length - 1);
        ActiveField.caretPosition = ActiveField.text.Length;
        ActiveField.ActivateInputField();
    }

    public void Clear()
    {
        if (ActiveField == null) return;

        ActiveField.text = "";
        ActiveField.ActivateInputField();
    }

    public void Enter()
    {
        // Optional: call login/submit logic here
        // Debug.Log("Enter pressed");
    }
}
