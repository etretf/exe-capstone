using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AudioRoomManager : MonoBehaviour
{
    public static AudioRoomManager Instance;
    
    [SerializeField] private AudioClip static_clip;
    [SerializeField] private AudioClip conversation_clip;
    [SerializeField] private AudioClip ring_clip;
    [SerializeField] private AudioClip phone_pickup_clip;
    [SerializeField] private AudioClip phone_place_down_clip;
    [SerializeField] private XRGrabExtension phone;
    [SerializeField] private XRSocketInteractor phone_base_socket;
    [SerializeField] private AudioSource audio_player;
    [SerializeField] private LevelDelays level_delays;
    [SerializeField] private InstructionalText instructional_text;
    [SerializeField] private AudioSource phone_base_audio_player;

    private bool is_audio_complete = false;
    private bool is_phone_picked_up = false;
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

    //destroy the phone. it seems to sometimes have parenting issues where it parents itself in the first heicharhy layer
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

    //play ringing audio before the player picks up the phone
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
            yield return new WaitForSeconds(level_delays.pause_between_phone_rings);
        }
    }

    //when the player picks up the phone
    public void PlayPickedUpPhoneAudio()
    {
        if (is_audio_complete)
            return;

        phone_base_audio_player.clip = phone_pickup_clip;
        phone_base_audio_player.Play();

        is_phone_picked_up = true;
        head_trigger = GameObject.FindWithTag(AllConstants.HEAD_TAG);
        head_trigger.GetComponent<Collider>().enabled = true;
        phone_base_socket.enabled = false;
        PlayStaticAudio();
    }

    //play static audio while the phone is away from head
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

    //play conversation audio while the phone is near head
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

    //enable phone base socket once the conversation completed playing
    private IEnumerator EnablePhoneBaseSocket (float audio_length)
    {
        yield return new WaitForSeconds(audio_length);
        Debug.Log("audio clip complete");
        is_audio_complete = true;
        phone_base_socket.enabled = true;
        instructional_text.SetTextValue("Press the grip button to place down the phone");
        PlayStaticAudio();

        if(head_trigger != null)
            head_trigger.GetComponent<Collider>().enabled = false;
    }

    //prevent phone pick up after it has been placed down
    public void DisablePhoneInteraction()
    { 
        if (is_audio_complete)
        {
            phone_base_audio_player.clip = phone_place_down_clip;
            phone_base_audio_player.Play();

            audio_player.Stop();
            StartCoroutine(DelayOpenDoor(2.0f));
            StartCoroutine(DisablePhone());
        }
    }

    private IEnumerator DelayOpenDoor(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.GetComponent<Room>().OpenDoor();
    }

    private IEnumerator DisablePhone()
    {
        yield return null;
        phone.GetComponent<XRGrabExtension>().enabled = false;
        phone_base_socket.enabled = false;
    }

    //disable or enable release. This is to prevent the phone from being released mid-air and only allowing release of it once inside the phone base XRSocketInteractor collider/trigger
    public void SetCanRelease(bool enable)
    {
        phone.GetComponent<XRGrabExtension>().SetAllowRelease(enable);
    }

    //display pick up instructional text
    public void DisplayPickUpText()
    {
        if(!is_phone_picked_up)
        {
            instructional_text.Show();
        }
    }

    //display place down instructional text 
    public void DisplayPlaceDownText()
    {
        if (is_audio_complete)
        {
            instructional_text.Show();
        }
    }

    //hide text
    public void HideText()
    {
        instructional_text.Hide();
    }
}
