using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AudioRoomManager : MonoBehaviour
{
    public static AudioRoomManager Instance;

    [SerializeField] private AudioClip static_clip;
    [SerializeField] private AudioClip conversation_clip;
    [SerializeField] private XRGrabInteractable phone;
    [SerializeField] private XRSocketInteractor phone_base_socket;
    [SerializeField] private AudioSource audio_player;

    private bool is_audio_complete = false;
    

    private float conversation_clip_time_passed = 0f;
    private float static_clip_time_passed = 0f;
    private Coroutine conversation_end;
    private GameObject head_trigger;
    private bool can_grab = true;
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
            //phone.interactionLayers = 0;
            //phone_base_socket.enabled = false;
            StartCoroutine(DisablePhone());
        }
    }

    private IEnumerator DisablePhone()
    {
        //for (int i = 0; i < 5; i++)
        //{
        //    yield return null; // wait one frame
        //}
        yield return null;
        phone.enabled = false;
        phone_base_socket.enabled = false;
        //phone.interactionLayers = 0;
        //phone_base_socket.interactionLayers = 0;
        //phone_base_socket.enabled = false;
    }

    public void SetCanRelease(bool enable)
    {
        can_grab = enable;
    }

    public bool GetCanRelease()
    {
        return can_grab;
    }
}
