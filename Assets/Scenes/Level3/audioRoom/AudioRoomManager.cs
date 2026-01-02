using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AudioRoomManager : MonoBehaviour
{
    public static AudioRoomManager Instance;

    [SerializeField] private AudioClip static_clip;
    [SerializeField] private AudioClip conversation_clip;
    [SerializeField] private XRSocketInteractor phone_base_socket;
    [SerializeField] private AudioSource audio_player;

    private bool is_audio_complete = false;
    

    private float conversation_clip_time_left = 0f;
    private float static_clip_time_left = 0f;
    private Coroutine conversation_end;
    private GameObject head_trigger;
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

    public void PlayAudio()
    {
        head_trigger = GameObject.FindWithTag("Head");
        head_trigger.GetComponent<Collider>().enabled = true;
        phone_base_socket.enabled = false;
        PlayStaticAudio();
    }

    public void PlayStaticAudio()
    {
        if (is_audio_complete)
            return;

        if (audio_player.clip != null && audio_player.clip == conversation_clip) 
            conversation_clip_time_left = audio_player.time;

        if(conversation_end != null)
            StopCoroutine(conversation_end);

        audio_player.clip = static_clip;
        audio_player.time = static_clip_time_left;
        audio_player.Play();
    }

    public void PlayConverstationAudio()
    {
        if (is_audio_complete)
            return;

        if (audio_player.clip != null && audio_player.clip == static_clip)
            static_clip_time_left = audio_player.time;

        audio_player.clip = conversation_clip;
        audio_player.time = conversation_clip_time_left;
        audio_player.Play();

        conversation_end = StartCoroutine(EnablePhoneBaseSocket(conversation_clip.length - conversation_clip_time_left));
    }

    private IEnumerator EnablePhoneBaseSocket (float audio_length)
    {
        yield return new WaitForSeconds(audio_length);
        Debug.Log("audio clip complete");
        is_audio_complete = true;
        phone_base_socket.enabled = true;

        if(head_trigger != null)
            head_trigger.GetComponent<Collider>().enabled = false;
    }
}
