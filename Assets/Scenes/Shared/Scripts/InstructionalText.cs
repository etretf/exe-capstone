using TMPro;
using UnityEngine;

public class InstructionalText : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject text_canvas;

    //methods to modify text properties through code
    public void SetTextValue(string value)
    {
        text.GetComponent<TextMeshProUGUI>().text = value;
    }
    public void SetFontSize(int value)
    {
        text.GetComponent<TextMeshProUGUI>().fontSize = value;
    }
    public void SetTextPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    public void SetTextRotation(Quaternion rotation)
    {
        gameObject.transform.rotation = rotation;
    }

    public void Show()
    {
        text_canvas.SetActive(true);
    }

    public void Hide()
    {
        text_canvas.SetActive(false);
    }
}
