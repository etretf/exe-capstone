using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class TTSDemo : MonoBehaviour
{
    public TTSSpeaker ttsSpeaker;
    public string playerName;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onSpeak(string name)
    {
        ttsSpeaker.Speak(name);
    }
}
