using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PhoneInteractions : MonoBehaviour
{
    public void DisplayInstructionalText(HoverEnterEventArgs args)
    {
        if(args.interactorObject is NearFarInteractor)
            AudioRoomManager.Instance.DisplayPickUpText();
    }

    public void HideInstructionalText()
    {
        AudioRoomManager.Instance.HideText();
    }
}
