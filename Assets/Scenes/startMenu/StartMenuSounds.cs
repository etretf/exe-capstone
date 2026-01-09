using System.Collections;
using UnityEngine;

public class StartMenuSounds : MonoBehaviour
{
    public static StartMenuSounds Instance;
    [SerializeField] AudioSource ambient_audio_src;
    [SerializeField] AudioSource computer_start_audio_src;
    [SerializeField] AudioClip no_static_computer_startup_sfx;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(startMusic());
    }

    private IEnumerator startMusic()
    {
        // Wait then play the startup sound
        yield return new WaitForSeconds(1f);
        computer_start_audio_src.Play();

        yield return new WaitForSeconds(computer_start_audio_src.clip.length - 1f);
        ambient_audio_src.Play();
    }

    public void playComputerStartSfx()
    {
        computer_start_audio_src.clip = no_static_computer_startup_sfx;
        computer_start_audio_src.Play();

        StartCoroutine(fadeAmbience());
    }

    private IEnumerator fadeAmbience()
    {
        if (ambient_audio_src.volume > 0)
        {
            ambient_audio_src.volume -= 0.02f;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(fadeAmbience());
        }
        else
        {
            yield return null;
        }
    }
}
