using System;
using System.Collections;
using UnityEngine;
using static LevelConstants;

public class Room : MonoBehaviour
{
    public Door door;
    private bool roomCompleted = false;
    [SerializeField] RoomType room_type;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.SetPlayerNotInHallway(); 

        if (door != null && !door.IsDoorClosed() && !roomCompleted)
        {
            door.CloseDoor();
            switch(room_type)
            {
                case RoomType.audio:
                    StartCoroutine(fadeSound());
                    break;
                default:
                    break;
            }
            
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

    IEnumerator fadeSound()
    {
        AmbientSoundManager.Instance.fadeOutAmbience();
        yield return new WaitForSeconds(2.0f);
        AudioRoomManager.Instance.PlayRingingAudio();
    }
}
