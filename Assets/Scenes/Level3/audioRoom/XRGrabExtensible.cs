using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRGrabExtension : XRGrabInteractable
{

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Re-select the object immediately to prevent mid-air release
        if (!AudioRoomManager.Instance.GetCanRelease() && args.interactorObject is NearFarInteractor)
        {
            args.manager.SelectEnter(args.interactorObject, args.interactableObject);
        }
    }
}
