using Meta.WitAi.TTS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public TTSSpeaker ttsSpeaker;
    public PlayerDataManager playerDataManager;
    private FaceAnimator faceAnimator;
    private string playerName;
    private float nameSpeakTime = 2.0f;


    [Header("Dialogue Input")]
    public List<Sprite> coderooniFacialExpressions = new List<Sprite>();

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

            delayBefore.text = "Before: " + line.delayBefore.ToString() + " s";
            delayAfter.text = "After: " + line.delayAfter.ToString() + " s";

            if (line.delayBefore > 0)
            {
                yield return new WaitForSeconds(line.delayBefore - 0.02f);
            }

            audioSource.clip = line.clip;
            audioSource.Play();
            facialExpressionImage.sprite = coderooniFacialExpressions[(int) line.expression];
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

        delayBefore.text = "";
        delayAfter.text = "";
        facialExpressionImage.sprite = null;

        faceAnimator.clearFacialExpression();
    }
}