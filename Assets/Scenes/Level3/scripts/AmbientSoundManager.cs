using System.Collections;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    public static AmbientSoundManager Instance;
    [SerializeField] AudioSource ambient_sound_src;
    private float ambient_sound_volume = 0.13f;

    private void Awake()
    {
        Instance = this;
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fadeInAmbience()
    {
        StartCoroutine(ambientSoundsFadeIn());
    }

    public void fadeOutAmbience()
    {
        Debug.Log("ambience manager fade out");
        StartCoroutine(ambientSoundsFadeOut());
    }

    public float getAudioSourceVolume()
    {
        return ambient_sound_src.volume;
    }

    IEnumerator ambientSoundsFadeOut()
    {
        if (ambient_sound_src.volume > 0)
        {
            ambient_sound_src.volume -= 0.01f;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(ambientSoundsFadeOut());
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator ambientSoundsFadeIn()
    {
        if (ambient_sound_src.volume < ambient_sound_volume)
        {
            ambient_sound_src.volume += 0.01f;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(ambientSoundsFadeIn());
        }
        else
        {
            yield return null;
        }
    }
}
