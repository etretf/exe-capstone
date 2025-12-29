using UnityEngine;

public class Door : MonoBehaviour
{
    private bool is_door_closed = true;
    private float original_door_pos_x;
    private float original_door_pos_y;
    private float original_door_pos_z;

    public void Start()
    {
        original_door_pos_x = gameObject.transform.position.x;
        original_door_pos_y = gameObject.transform.position.y;
        original_door_pos_z = gameObject.transform.position.z;
    }

    //opens the door
    //TODO: if there is time randomize the animation each time to create variety
    public void OpenDoor()
    {
        if(is_door_closed)
        {
            gameObject.transform.position = new Vector3(original_door_pos_x, original_door_pos_y, original_door_pos_z + 1.6f);
            is_door_closed = false;
        }
    }

    //closes the door
    //TODO: if there is time randomize the animation each time to create variety
    public void CloseDoor()
    {
        if(!is_door_closed)
        {
            gameObject.transform.position = new Vector3(original_door_pos_x, original_door_pos_y, original_door_pos_z);
            is_door_closed = true;
        }
    }

    //helper method to check if a door is closed or open
    public bool IsDoorClosed()
    {
        return is_door_closed;
    }
}
