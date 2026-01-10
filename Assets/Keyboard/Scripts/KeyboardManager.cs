//using UnityEngine;
//using TMPro;

//public class KeyboardManager : MonoBehaviour
//{
//    public static KeyboardManager Instance { get; private set; }

//    [Header("Optional default field (if you want)")]
//    public TMP_InputField defaultField;

//    public TMP_InputField ActiveField { get; private set; }

//    void Awake()
//    {
//        // Simple singleton so keys + fields can talk to one keyboard
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this;
//    }

//    void Start()
//    {
//        if (defaultField != null)
//            SetActiveField(defaultField);
//    }

//    public void SetActiveField(TMP_InputField field)
//    {
//        ActiveField = field;
//        if (ActiveField != null)
//            ActiveField.ActivateInputField();
//    }

//    public void Type(string text)
//    {
//        if (ActiveField == null) return;

//        ActiveField.text += text;
//        ActiveField.caretPosition = ActiveField.text.Length;
//        ActiveField.ActivateInputField();
//    }

//    public void Backspace()
//    {
//        if (ActiveField == null) return;

//        string t = ActiveField.text;
//        if (t.Length == 0) return;

//        ActiveField.text = t.Substring(0, t.Length - 1);
//        ActiveField.caretPosition = ActiveField.text.Length;
//        ActiveField.ActivateInputField();
//    }

//    public void Clear()
//    {
//        if (ActiveField == null) return;

//        ActiveField.text = "";
//        ActiveField.ActivateInputField();
//    }

//    public void Enter()
//    {
//        // Optional: call login/submit logic here
//        // Debug.Log("Enter pressed");
//    }
//}
using UnityEngine;
using TMPro;
using System;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager Instance { get; private set; }

    [Header("Deafult field (its optional not needed to add)")]
    public TMP_InputField defaultField;

    [Header("Login fields (drag from your monitor UI)")]
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;

    [Header("Panels to swap on correct password")]
    public GameObject loginPanel;
    public GameObject nextPanel;

    private string correctPassword = "1234";

    public TMP_InputField ActiveField { get; private set; }

    public bool CapsLockOn { get; private set; } = false;
    public event Action<bool> OnCapsLockChanged;

    void Awake()
    {
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

        if (loginPanel != null) loginPanel.SetActive(true);
        if (nextPanel != null) nextPanel.SetActive(false);
    }

    public void SetActiveField(TMP_InputField field)
    {
        ActiveField = field;
        if (ActiveField != null)
            ActiveField.ActivateInputField();
    }

    public void ToggleCapsLock()
    {
        CapsLockOn = !CapsLockOn;
        OnCapsLockChanged?.Invoke(CapsLockOn);
    }

    public void Type(string raw)
    {
        if (ActiveField == null || string.IsNullOrEmpty(raw)) return;

        raw = raw.Trim();         
        if (raw.Length == 0) return;

        if (raw.Length == 1)
        {
            char c = raw[0];
            if (char.IsLetter(c))
                raw = CapsLockOn ? char.ToUpperInvariant(c).ToString()
                                 : char.ToLowerInvariant(c).ToString();
        }

        ActiveField.text += raw;
        ActiveField.caretPosition = ActiveField.text.Length;
        ActiveField.ActivateInputField();
    }


    public void Backspace()
    {
        if (ActiveField == null) return;

        string t = ActiveField.text;
        if (string.IsNullOrEmpty(t)) return;

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
        if (passwordField == null || ActiveField != passwordField) return;

        string entered = passwordField.text.Trim();
        string expected = correctPassword.Trim();

        if (entered == expected)
        {
            Debug.Log("Welcome you put the right password!");
            if (loginPanel != null) loginPanel.SetActive(false);
            if (nextPanel != null) nextPanel.SetActive(true);
        }
        else
        {
            Debug.Log($"Wrong password. Entered:'{entered}' (len {entered.Length}) Expected:'{expected}' (len {expected.Length})");
        }
    }

}
