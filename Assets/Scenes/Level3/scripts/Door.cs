using UnityEngine;
using System.Collections;
using System;

public class Door : MonoBehaviour
{
    public LevelDelays level_delays;
    [SerializeField] private Canvas control_instructions;

    private bool is_door_closed = true;
    private float original_door_pos_x;
    private float original_door_pos_y;
    private float original_door_pos_z;
    private Coroutine delayDoorOpenCoroutine;

    private AudioSource audio_src;
    public AudioClip delay_door_sfx;
    public AudioClip door_open_sfx;
    public AudioClip door_close_sfx;

    public void Start()
    {
        original_door_pos_x = gameObject.transform.position.x;
        original_door_pos_y = gameObject.transform.position.y;
        original_door_pos_z = gameObject.transform.position.z;
        audio_src = gameObject.GetComponent<AudioSource>();
    }

    // Opens the door
    // TODO: if there is time randomize the animation each time to create variety
    public void OpenDoor()
    {
        if(is_door_closed)
        {
            is_door_closed = false;
            delayDoorOpenCoroutine = StartCoroutine(DelayDoorOpen());
        }
    }

    // Closes the door
    // TODO: if there is time randomize the animation each time to create variety
    public void CloseDoor()
    {
        if(!is_door_closed)
        {
            audio_src.clip = door_close_sfx;
            audio_src.Stop();
            StopCoroutine(delayDoorOpenCoroutine);
            delayDoorOpenCoroutine = null;
            gameObject.transform.position = new Vector3(original_door_pos_x, original_door_pos_y, original_door_pos_z);
            is_door_closed = true;
        }
    }

    //show door instructions
    public void DisplayDoorControls()
    {
        if(is_door_closed && GameManager.Instance.GetIsPlayerInHallway())
        {
            control_instructions.gameObject.SetActive(true);
        }
    }

    //hide door instructions
    public void HideDoorControls()
    {
        control_instructions.gameObject.SetActive(false);
    }

    // open door after 3 seconds
    IEnumerator DelayDoorOpen()
    {
        audio_src.clip = delay_door_sfx;
        audio_src.Play();
        
        yield return new WaitForSeconds(level_delays.door_open_delay_seconds);
        gameObject.transform.position = new Vector3(original_door_pos_x, original_door_pos_y, original_door_pos_z + 1.6f);

        audio_src.clip = door_open_sfx;
        audio_src.Play();
    }

    // helper method to check if a door is closed or open
    public bool IsDoorClosed()
    {
        return is_door_closed;
    }
}
