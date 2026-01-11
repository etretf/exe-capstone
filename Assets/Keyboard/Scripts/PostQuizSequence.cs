using System.Collections;
using UnityEngine;

public class PostQuizSequence : MonoBehaviour
{
    [Header("Panels")]
    public GameObject resultsPanel;
    public GameObject downloadPanel;
    public GameObject coderooniPanel;

    [Header("Boss UI")]
    public GameObject bossSlackPopup;     
    public GameObject bossMessagingPanel; 

    [Header("Timing")]
    public float coderooniDelay = 5f; 

    void Start()
    {
  
        if (downloadPanel) downloadPanel.SetActive(false);
        if (coderooniPanel) coderooniPanel.SetActive(false);
        if (bossSlackPopup) bossSlackPopup.SetActive(false);
        if (bossMessagingPanel) bossMessagingPanel.SetActive(false);
    }

    public void GoToDownload()
    {
        if (resultsPanel) resultsPanel.SetActive(false);

        if (downloadPanel) downloadPanel.SetActive(true);
        if (coderooniPanel) coderooniPanel.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(ShowCoderooniAfterDelay());
    }

    IEnumerator ShowCoderooniAfterDelay()
    {
        yield return new WaitForSeconds(coderooniDelay);

        if (downloadPanel != null && !downloadPanel.activeInHierarchy)
            yield break;

        if (coderooniPanel) coderooniPanel.SetActive(true);
    }

   
}
