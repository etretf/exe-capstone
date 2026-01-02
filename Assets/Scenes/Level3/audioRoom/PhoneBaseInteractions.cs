using UnityEngine;

public class PhoneBaseInteractions : MonoBehaviour
{
    public void PickUpPhone()
    {
        AudioRoomManager.Instance.PlayAudio();
    }

    //TODO
    public void PlaceDownPhone()
    {
        AudioRoomManager.Instance.PlayAudio();
    }
}
