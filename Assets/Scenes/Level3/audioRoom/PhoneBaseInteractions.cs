using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PhoneBaseInteractions : MonoBehaviour
{
    public void PickUpPhone()
    {
        AudioRoomManager.Instance.PlayAudio();
    }

    public void PlaceDownPhone()
    {
        AudioRoomManager.Instance.DisablePhoneInteraction();
    }

    public void DisableRelease()
    {
        AudioRoomManager.Instance.SetCanRelease(false);
    }
    public void EnableRelease()
    {
        AudioRoomManager.Instance.SetCanRelease(true);
    }
}

