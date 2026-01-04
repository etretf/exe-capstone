using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRGrabExtension : XRGrabInteractable
{
    private IXRSelectInteractor released_interactor;
    private bool allow_release = true;
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        //check if this is the player trying to pass the obejct between two hands. If yes then allow release of object from the previous hand and release it
        if (!allow_release && args.interactorObject is NearFarInteractor && released_interactor != null && args.interactorObject != released_interactor)
        {
            allow_release = true;
            args.manager.SelectExit(released_interactor, args.interactableObject);
            args.manager.SelectEnter(args.interactorObject, args.interactableObject);
            allow_release = false;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Re-select the object immediately to prevent mid-air release if release is currently blocked
        if (!allow_release && args.interactorObject is NearFarInteractor)
        {
            released_interactor = args.interactorObject;
            args.manager.SelectEnter(args.interactorObject, args.interactableObject);
        }
    }

    public void SetAllowRelease(bool value) { 
        allow_release = value;
    }
}
