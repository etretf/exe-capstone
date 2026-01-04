using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PhoneBaseInteractions : MonoBehaviour
{
    public void PickUpPhone()
    {
        AudioRoomManager.Instance.PlayPickedUpPhoneAudio();
    }

    public void PlaceDownPhone()
    {
        AudioRoomManager.Instance.DisablePhoneInteraction();
    }

    public void DisableRelease()
    {
        AudioRoomManager.Instance.SetCanRelease(false);
        AudioRoomManager.Instance.HideText();
    }
    public void EnableRelease()
    {
        AudioRoomManager.Instance.SetCanRelease(true);
        AudioRoomManager.Instance.DisplayPlaceDownText();
    }
}

