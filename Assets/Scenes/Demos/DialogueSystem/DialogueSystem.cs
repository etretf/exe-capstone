using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    public static DialogueSystem instance;

    [Header("Dependencies")]
    public AudioSource audioSource;
    private FaceAnimator faceAnimator;


    [Header("Dialogue Input")]
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
        dialogueRoutine = StartCoroutine(playDialogueLines());
    }

    public void clearDialogueLines()
    {
        dialogueLines.Clear();
    }

    IEnumerator playDialogueLines()
    {
        for (int i = 0; i < dialogueLines.Count; i++)
        {
            DialogueLine line = dialogueLines[i];

            if (line.delayBefore > 0)
            {
                yield return new WaitForSeconds(line.delayBefore);
            }

            audioSource.clip = line.clip;
            audioSource.Play();
            faceAnimator.setFaceSprite(line.expression);

            yield return new WaitForSeconds(line.clip.length);

            if (line.delayAfter > 0)
            {
                yield return new WaitForSeconds(line.delayAfter);
            }
        }

        faceAnimator.clearFacialExpression();
    }
}

[Serializable] public class DialogueLine
{
    public CoderooniConstants CoderooniConstants;
    public AudioClip clip;
    public CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS expression;
    public bool speakPlayerNameAfter = false;

    [Header("Timing settings")]
    public float delayBefore = 0.0f;
    public float delayAfter = 0.0f;
}