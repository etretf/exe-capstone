using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorActions : MonoBehaviour
{
    public void Create()
    {
        if (!RoomManager.Instance.DoesRoomExist())
        {
            RoomManager.Instance.CreateRoom();
        }
    }

    public void Destroy()
    {
        if (RoomManager.Instance.DoesRoomExist())
        {
            RoomManager.Instance.DestroyRoom();
        }
    }

    public void OpenDoor()
    {
        gameObject.GetComponent<Door>().OpenDoor();
    }
}
