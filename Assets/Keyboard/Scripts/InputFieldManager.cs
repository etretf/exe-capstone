using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldManager : MonoBehaviour, IPointerClickHandler, ISelectHandler
{
    private TMP_InputField _field;

    void Awake()
    {
        _field = GetComponent<TMP_InputField>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetActive();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetActive();
    }

    private void SetActive()
    {
        if (KeyboardManager.Instance != null)
            KeyboardManager.Instance.SetActiveField(_field);
    }
}
