using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorActions : LevelDoorActions
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

    public override void OpenDoor()
    {
        gameObject.GetComponent<HallwayDoor>().OpenDoor();
        if (TileGenerationManager.Instance.GetRoomTypeAtPlayerLocation() == LevelConstants.RoomType.correct) {
            GoToNextLevel();
        } 
    }
}
