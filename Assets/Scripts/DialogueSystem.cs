using Meta.WitAi.TTS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;

    [Header("Dependencies")]
    public AudioSource audioSource;
    public TTSSpeaker ttsSpeaker;
    public PlayerDataManager playerDataManager;
    private FaceAnimator faceAnimator;
    private string playerName;
    private float nameSpeakTime = 2.0f;


    [Header("Dialogue Input")]
    public List<DialogueLines> dialogueBundles = new List<DialogueLines>();

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
        playerName = playerDataManager.playerName;
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

    public void startDialogue(int idx)
    {
        // Trigger a bundle of dialogue lines in the list by the index
        // A bundle is a list of dialogue lines
        dialogueRoutine = StartCoroutine(playDialogueLines(idx));
    }

    IEnumerator playDialogueLines(int idx)
    {
        List<DialogueLine> lines = dialogueBundles[idx].lines;
        for (int i = 0; i < lines.Count; i++)
        {
            DialogueLine line = lines[i];

            if (line.delayBefore > 0)
            {
                yield return new WaitForSeconds(line.delayBefore - 0.02f);
            }

            audioSource.clip = line.clip;
            audioSource.Play();
            faceAnimator.setFaceSprite(line.expression);

            yield return new WaitForSeconds(line.clip.length);

            if (line.speakPlayerNameAfter == true)
            {
                ttsSpeaker.Speak(playerName);
                yield return new WaitForSeconds(nameSpeakTime);
            }

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

[System.Serializable] public class DialogueLines
{
    public List<DialogueLine> lines;
}