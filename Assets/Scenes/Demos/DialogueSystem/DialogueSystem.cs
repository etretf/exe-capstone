using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    public static DialogueSystem instance;

    [Header("Dependencies")]
    public AudioSource audioSource;
    public Image facialExpressionImage;
    public TextMeshProUGUI delayBefore;
    public TextMeshProUGUI delayAfter;


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

    public void stopDialogue()
    {
        StopCoroutine(dialogueRoutine);
        audioSource.Stop();
    }

    public void startDialogue()
    {
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


            yield return new WaitForSeconds(line.clip.length);

            if (line.delayAfter > 0)
            {
                yield return new WaitForSeconds(line.delayAfter);
            }
        }

        delayBefore.text = "";
        delayAfter.text = "";
        facialExpressionImage.sprite = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum CODEROONI_FACIAL_EXPRESSIONS
{
    happy_1, happy_hearts_2, happy_uwu_3, annoyed_4, dead_5, shocked_6, sad_7, offended_8, ominous_9
}

[Serializable] public class DialogueLine
{
    public AudioClip clip;
    public CODEROONI_FACIAL_EXPRESSIONS expression;

    [Header("Timing settings")]
    public float delayBefore = 0.0f;
    public float delayAfter = 0.0f;
}