using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using System.Collections;
using Meta.WitAi.TTS.Data;
using System;
using Meta.WitAi.TTS;

public class MetaVoiceSequencePlayer : MonoBehaviour
{
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip fetchedClip;
    bool ttsPlayed = false;
    public string nameText = "Emma";

    public AudioSource audioSource;
    public TTSSpeaker ttsSpeaker;

    void Awake()
    {
        ttsSpeaker.Events.OnPlaybackComplete.AddListener(onTTSPlayed);
    }

    private void onTTSPlayed(TTSSpeaker arg0, TTSClipData arg1)
    {
        ttsPlayed = true;
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


        ttsPlayed=false;
    }

    void playAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.PlayOneShot(clip);
    }

}
