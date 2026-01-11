using UnityEngine;
using UnityEngine.UI;

public class BossPopupSequence : MonoBehaviour
{
    [Header("Boss popup UI")]
    public GameObject bossSlackPopup;     // root popup object (slackPopup (1))
    public Button openButton;            // "Open" button on the popup

    [Header("Where it goes")]
    public GameObject bossMessagingPanel; // messagingPanel (1)

    void Awake()
    {
        // Start hidden
        if (bossSlackPopup != null)
            bossSlackPopup.SetActive(false);

        if (bossMessagingPanel != null)
            bossMessagingPanel.SetActive(false);

        if (openButton != null)
            openButton.onClick.AddListener(OpenBossMessages);
    }

    // Call this from Coderooni Finish
    public void ShowBossPopup()
    {
        if (bossSlackPopup != null)
            bossSlackPopup.SetActive(true);
    }

    public void HideBossPopup()
    {
        if (bossSlackPopup != null)
            bossSlackPopup.SetActive(false);
    }

    void OpenBossMessages()
    {
        HideBossPopup();

        if (bossMessagingPanel != null)
            bossMessagingPanel.SetActive(true);
    }
}
