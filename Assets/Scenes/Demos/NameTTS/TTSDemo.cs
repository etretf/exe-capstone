using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using System.Collections;
using Meta.WitAi.TTS.Data;
using System;
using Meta.WitAi.TTS;
using UnityEngine.TextCore.Text;
using System.Text;

public class DialogueTTSSpeaker : MonoBehaviour
{
    public AudioClip clip1;
    public AudioClip clip2;
    public string nameText = "Emma";

    public AudioSource audioSource;
    public TTSSpeaker ttsSpeaker;

    void Awake()
    {
    }

    void Start()
    {
    }

    public void speakNameBetweenPhrases(string name)
    {
        StartCoroutine(playAudioClips());
    }

    IEnumerator playAudioClips()
    {
        playAudioClip(clip1);
        yield return new WaitForSeconds(clip1.length);

        ttsSpeaker.Speak(nameText);
        yield return new WaitForSeconds(1.5f);

        playAudioClip(clip2);
    }

    void playAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.PlayOneShot(clip);
    }

}
