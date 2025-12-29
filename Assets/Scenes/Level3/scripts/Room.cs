using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Door door;


    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.SetPlayerNotInHallway(); 

        if (door != null && !door.IsDoorClosed())
        {
            door.CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if (door != null && door.IsDoorClosed()) 
        {
            door.OpenDoor();
        }
        
    }
}
