using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessageSequence : MonoBehaviour
{
    public GameObject msg1;
    public GameObject msg2;
    public GameObject msg3Quiz;

    public float delayMsg1 = 0.6f;
    public float delayMsg2 = 1.2f;
    public float delayMsg3 = 1.2f;

    public Button quizButton; 


    public GameObject quizPanel;     
    public GameObject messagePanel; 
    Coroutine routine;

    void OnEnable()
    {
        StartSequence();
    }

    public void StartSequence()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // reset
        if (msg1 != null) msg1.SetActive(false);
        if (msg2 != null) msg2.SetActive(false);
        if (msg3Quiz != null) msg3Quiz.SetActive(false);

        yield return new WaitForSeconds(delayMsg1);
        if (msg1 != null) msg1.SetActive(true);

        yield return new WaitForSeconds(delayMsg2);
        if (msg2 != null) msg2.SetActive(true);

        yield return new WaitForSeconds(delayMsg3);
        if (msg3Quiz != null) msg3Quiz.SetActive(true);

        // hook quiz button (safe)
        if (quizButton != null)
        {
            quizButton.onClick.RemoveAllListeners();
            quizButton.onClick.AddListener(OpenQuiz);
        }
    }

    void OpenQuiz()
    {
        if (quizPanel != null)
            quizPanel.SetActive(true);

        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}
