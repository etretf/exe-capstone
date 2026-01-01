using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemDemo : MonoBehaviour
{
    public static DialogueSystemDemo instance;

    [Header("Dependencies")]
    public AudioSource audioSource;
    public Image facialExpressionImage;
    public TextMeshProUGUI delayBefore;
    public TextMeshProUGUI delayAfter;
    private FaceAnimator faceAnimator;


    [Header("Dialogue Input")]
    public List<Sprite> coderooniFacialExpressions = new List<Sprite>();

    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    private Coroutine dialogueRoutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        faceAnimator = GetComponent<FaceAnimator>();
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {

    }


    public void stopDialogue()
    {
        StopCoroutine(dialogueRoutine);
        audioSource.Stop();
    }

    public void startDialogue()
    {
        Debug.Log("start dialogue");
        dialogueRoutine = StartCoroutine(playDialogueLines());
    }

    IEnumerator playDialogueLines()
    {
        for (int i = 0; i < dialogueLines.Count; i++)
        {
            DialogueLine line = dialogueLines[i];

            delayBefore.text = "Before: " + line.delayBefore.ToString() + " s";
            delayAfter.text = "After: " + line.delayAfter.ToString() + " s";

            if (line.delayBefore > 0)
            {
                yield return new WaitForSeconds(line.delayBefore);
            }

            audioSource.clip = line.clip;
            audioSource.Play();
            facialExpressionImage.sprite = coderooniFacialExpressions[(int) line.expression];
            faceAnimator.setFaceSprite(line.expression);

            yield return new WaitForSeconds(line.clip.length);

            if (line.delayAfter > 0)
            {
                yield return new WaitForSeconds(line.delayAfter);
            }
        }

        delayBefore.text = "";
        delayAfter.text = "";
        facialExpressionImage.sprite = null;

        faceAnimator.clearFacialExpression();
    }
}