using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class KeyboardKeyButton : MonoBehaviour
{
    private Button _button;
    private TextMeshProUGUI _label;

    void Awake()
    {
        _button = GetComponent<Button>();
        _label = GetComponentInChildren<TextMeshProUGUI>(true);

        _button.onClick.AddListener(OnPressed);
    }

    void OnPressed()
    {
        if (KeyboardManager.Instance == null) return;
        if (KeyboardManager.Instance.ActiveField == null) return; // must click a field first

        string key = _label != null ? _label.text : "";
        if (string.IsNullOrEmpty(key)) return;

        // Special keys by label text
        string lower = key.ToLower();

        if (key == "⌫" || lower == "back" || lower == "delete")
            KeyboardManager.Instance.Backspace();
        else if (key == "⏎" || lower == "enter")
            KeyboardManager.Instance.Enter();
        else if (lower == "clr" || lower == "clear")
            KeyboardManager.Instance.Clear();
        else
            KeyboardManager.Instance.Type(key);
    }
}
