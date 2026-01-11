using UnityEngine;
using UnityEngine.UI;

public class SlackPopupController : MonoBehaviour
{
    public GameObject slackPopup;
    public Button slackPopupButton;
    public float autoShowDelay = 4f;
    public GameObject mainScreen;          // your desktop screen (nextPanel / desktop)

    public GameObject messagingPanel;      // the messaging app panel

    public System.Action OnPopupClicked;

    void Start()
    {
        if (slackPopup != null)
            slackPopup.SetActive(false);

        if (messagingPanel != null)
            messagingPanel.SetActive(false);   // start hidden

        if (slackPopupButton != null)
            slackPopupButton.onClick.AddListener(OnSlackOpenClicked);

        InvokeRepeating(nameof(TrySchedulePopup), 0.1f, 0.1f);
    }

    void OnSlackOpenClicked()
    {
        HidePopup();

        // Show messaging panel
        if (messagingPanel != null)
            messagingPanel.SetActive(true);
        if (mainScreen != null)
            mainScreen.SetActive(false);

        OnPopupClicked?.Invoke();
    }

    void TrySchedulePopup()
    {
        if (mainScreen == null) return;
        if (!mainScreen.activeInHierarchy) return;

        CancelInvoke(nameof(TrySchedulePopup));
        Invoke(nameof(ShowPopup), autoShowDelay);
    }

    public void ShowPopup()
    {
        if (mainScreen != null && mainScreen.activeInHierarchy && slackPopup != null)
            slackPopup.SetActive(true);
    }

    public void HidePopup()
    {
        if (slackPopup != null)
            slackPopup.SetActive(false);
    }
}
