using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class PuzzlePiece : MonoBehaviour
{
    public int pieceId;            // 0..3 for 2x2
    public Material pieceMaterial; // assign in inspector

    Rigidbody _rb;
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grab;
    Renderer _renderer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    void Start()
    {
        if (_renderer && pieceMaterial)
            _renderer.sharedMaterial = pieceMaterial;
    }

    public void LockInPlace()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints.FreezeAll;

        StartCoroutine(DisableGrabNextFrame());
    }

    System.Collections.IEnumerator DisableGrabNextFrame()
    {
        yield return null;
        _grab.enabled = false;
    }
}
