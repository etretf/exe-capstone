using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Door door;
    private bool roomCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.SetPlayerNotInHallway(); 

        if (door != null && !door.IsDoorClosed() && !roomCompleted)
        {
            door.CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if (door != null && door.IsDoorClosed()) 
        {
            door.OpenDoor();
            roomCompleted = true;
        }
        
    }
}
