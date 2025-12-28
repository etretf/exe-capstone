using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CreateRoom : MonoBehaviour
{
    public void Create()
    {
        RoomManager.Instance.CreateRoom();
    }
}
