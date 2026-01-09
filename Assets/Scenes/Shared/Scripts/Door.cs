using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField] protected Canvas control_instructions;

    protected bool is_door_closed = true;
    protected float original_door_pos_x;
    protected float original_door_pos_y;
    protected float original_door_pos_z;
    protected Coroutine delayDoorOpenCoroutine;

    protected AudioSource audio_src;
    public AudioClip delay_door_sfx;
    public AudioClip door_open_sfx;
    public AudioClip door_close_sfx;

    public void Start()
    {
        original_door_pos_x = gameObject.transform.localPosition.x;
        original_door_pos_y = gameObject.transform.localPosition.y;
        original_door_pos_z = gameObject.transform.localPosition.z;
        audio_src = gameObject.GetComponent<AudioSource>();
    }

    // Opens the door
    // TODO: if there is time randomize the animation each time to create variety
    public void OpenDoor()
    {
        if (is_door_closed)
        {
            is_door_closed = false;
            delayDoorOpenCoroutine = StartCoroutine(DelayDoorOpen());
        }
    }

    // Closes the door
    // TODO: if there is time randomize the animation each time to create variety
    public void CloseDoor()
    {
        if (!is_door_closed)
        {
            audio_src.clip = door_close_sfx;
            audio_src.Play();
            StopCoroutine(delayDoorOpenCoroutine);
            delayDoorOpenCoroutine = null;
            gameObject.transform.localPosition = new Vector3(original_door_pos_x, original_door_pos_y, original_door_pos_z);
            is_door_closed = true;
        }
    }

    //show door instructions
    public virtual void DisplayDoorControls()
    {
        control_instructions.gameObject.SetActive(true);
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

        yield return new WaitForSeconds(AllConstants.DOOR_DELAY);
        gameObject.transform.localPosition = new Vector3(original_door_pos_x, original_door_pos_y, original_door_pos_z + 1.6f);

        audio_src.clip = door_open_sfx;
        audio_src.Play();
    }

    // helper method to check if a door is closed or open
    public bool IsDoorClosed()
    {
        return is_door_closed;
    }
}
