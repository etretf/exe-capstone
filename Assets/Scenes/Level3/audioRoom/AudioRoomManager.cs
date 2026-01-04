using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AudioRoomManager : MonoBehaviour
{
    public static AudioRoomManager Instance;
    const float PAUSE_BETWEEN_RINGS = 1f;

    [SerializeField] private AudioClip static_clip;
    [SerializeField] private AudioClip conversation_clip;
    [SerializeField] private AudioClip ring_clip;
    [SerializeField] private XRGrabExtension phone;
    [SerializeField] private XRSocketInteractor phone_base_socket;
    [SerializeField] private AudioSource audio_player;

    private bool is_audio_complete = false;
    

    private float conversation_clip_time_passed = 0f;
    private float static_clip_time_passed = 0f;
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

    private void OnDestroy()
    {
        if (phone == null) 
            return;

        if (phone.isSelected && phone.firstInteractorSelecting != null)
        {
            IXRSelectInteractor interactor = phone.firstInteractorSelecting;
            phone.interactionManager.SelectExit(interactor, phone);
        }

        Destroy(phone.gameObject);
        if (phone != null)
        {
            Destroy(phone);
        }
    }

    public void PlayRingingAudio()
    {
        audio_player.clip = ring_clip;
        audio_player.loop = false;
        StartCoroutine(LoopRinging());
    }

    private IEnumerator LoopRinging()
    {
        while (true)
        {
            audio_player.Play();
            yield return new WaitForSeconds(ring_clip.length);
            yield return new WaitForSeconds(PAUSE_BETWEEN_RINGS);
        }
    }

    public void PlayPickedUpPhoneAudio()
    {
        if (is_audio_complete)
            return;

        head_trigger = GameObject.FindWithTag("Head");
        head_trigger.GetComponent<Collider>().enabled = true;
        phone_base_socket.enabled = false;
        PlayStaticAudio();
    }

    public void PlayStaticAudio()
    {
        if (audio_player.clip != null && audio_player.clip == conversation_clip) 
            conversation_clip_time_passed = audio_player.time;

        StopAllCoroutines();
           
        audio_player.clip = static_clip;
        audio_player.loop = true;
        audio_player.time = static_clip_time_passed;
        audio_player.Play();
    }

    public void PlayConverstationAudio()
    {
        if (is_audio_complete)
            return;

        if (audio_player.clip != null && audio_player.clip == static_clip)
            static_clip_time_passed = audio_player.time;

        audio_player.clip = conversation_clip;
        audio_player.loop = false;
        audio_player.time = conversation_clip_time_passed;
        audio_player.Play();

        StartCoroutine(EnablePhoneBaseSocket(conversation_clip.length - conversation_clip_time_passed));
        
    }

    private IEnumerator EnablePhoneBaseSocket (float audio_length)
    {
        yield return new WaitForSeconds(audio_length);
        Debug.Log("audio clip complete");
        is_audio_complete = true;
        phone_base_socket.enabled = true;
        PlayStaticAudio();

        if(head_trigger != null)
            head_trigger.GetComponent<Collider>().enabled = false;
    }

    //helper function to check diable phone interaction after it has been placed down
    public void DisablePhoneInteraction()
    { 
        if (is_audio_complete)
        {
            audio_player.Stop();
            gameObject.GetComponent<Room>().OpenDoor();
            StartCoroutine(DisablePhone());
        }
    }

    private IEnumerator DisablePhone()
    {
        yield return null;
        phone.GetComponent<XRGrabExtension>().enabled = false;
        phone_base_socket.enabled = false;
    }

    public void SetCanRelease(bool enable)
    {
        phone.GetComponent<XRGrabExtension>().SetAllowRelease(enable);
    }
}
