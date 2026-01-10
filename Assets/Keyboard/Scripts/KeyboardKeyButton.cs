//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//[RequireComponent(typeof(Button))]
//public class KeyboardKeyButton : MonoBehaviour
//{
//    private Button _button;
//    private TextMeshProUGUI _label;

//    void Awake()
//    {
//        _button = GetComponent<Button>();
//        _label = GetComponentInChildren<TextMeshProUGUI>(true);

//        _button.onClick.AddListener(OnPressed);
//    }

//    void OnPressed()
//    {
//        if (KeyboardManager.Instance == null) return;
//        if (KeyboardManager.Instance.ActiveField == null) return; // must click a field first

//        string key = _label != null ? _label.text : "";
//        if (string.IsNullOrEmpty(key)) return;

//        // Special keys by label text
//        string lower = key.ToLower();

//        if (key == "⌫" || lower == "back" || lower == "delete")
//            KeyboardManager.Instance.Backspace();
//        else if (key == "⏎" || lower == "enter")
//            KeyboardManager.Instance.Enter();
//        else if (lower == "clr" || lower == "clear")
//            KeyboardManager.Instance.Clear();
//        else
//            KeyboardManager.Instance.Type(key);
//    }
//}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class KeyboardKeyButton : MonoBehaviour
{
    [Header("Optional override. Leave empty to use the button label text.")]
    public string overrideKey;

    [Header("Set TRUE only on the Caps key button")]
    public bool isCapsKey = false;

    [Header("Caps key color when ON")]
    public Color capsOnColor = new Color(0.2f, 0.8f, 0.2f, 1f);

    private Button _button;
    private TextMeshProUGUI _label;
    private Image _image;
    private Color _capsOffColor;

    void Awake()
    {
        _button = GetComponent<Button>();
        _label = GetComponentInChildren<TextMeshProUGUI>(true);
        _image = GetComponent<Image>();

        if (_image != null) _capsOffColor = _image.color;

        _button.onClick.AddListener(OnPressed);
    }

    void OnEnable()
    {
        // If this is the caps key, listen for caps changes to update color
        if (isCapsKey && KeyboardManager.Instance != null)
        {
            KeyboardManager.Instance.OnCapsLockChanged += UpdateCapsVisual;
            UpdateCapsVisual(KeyboardManager.Instance.CapsLockOn);
        }
    }

    void OnDisable()
    {
        if (isCapsKey && KeyboardManager.Instance != null)
            KeyboardManager.Instance.OnCapsLockChanged -= UpdateCapsVisual;
    }

    void OnPressed()
    {
        var km = KeyboardManager.Instance;
        if (km == null) return;

        // Must click an input field first
        if (km.ActiveField == null) return;

        string key = !string.IsNullOrEmpty(overrideKey)
            ? overrideKey
            : (_label != null ? _label.text : "");

        if (string.IsNullOrEmpty(key)) return;

        string norm = key.Trim().ToLowerInvariant();

        // CAPS
        if (isCapsKey || norm == "caps" || norm == "capslock" || key == "⇪")
        {
            km.ToggleCapsLock();
            return;
        }

        // DELETE / BACKSPACE
        if (norm == "delete" || norm == "del" || norm == "back" || key == "⌫")
        {
            km.Backspace();
            return;
        }

        // ENTER
        if (norm == "enter" || key == "⏎")
        {
            km.Enter();
            return;
        }

        // SPACE (label your space key "Space" or overrideKey=" ")
        if (norm == "space")
        {
            km.Type(" ");
            return;
        }

        // Normal key
        km.Type(key);
    }

    private void UpdateCapsVisual(bool capsOn)
    {
        if (_image == null) return;
        _image.color = capsOn ? capsOnColor : _capsOffColor;
    }
}
