using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorActions : MonoBehaviour
{
    private bool is_room_created = false;
    public void Create()
    {
        if (!is_room_created)
        {
            RoomManager.Instance.CreateRoom();
            is_room_created = true;
        }
    }

    public void Destroy()
    {
        if (is_room_created)
        {
            RoomManager.Instance.DestroyRoom();
            is_room_created = false;
        }
    }

    public void OpenDoor()
    {
        gameObject.GetComponent<Door>().OpenDoor();
    }
}
