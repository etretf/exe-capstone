using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor))]
public class PuzzleSocket : MonoBehaviour
{
    public int socketId; // must match correct pieceId
    public float rejectForce = 2f;

    public UnityEvent onCorrectPlaced;
    public UnityEvent onWrongPlaced;

    UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor _socket;
    PuzzlePiece _current;

    void Awake() => _socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();

    void OnEnable()
    {
        _socket.selectEntered.AddListener(OnSelectEntered);
        _socket.selectExited.AddListener(OnSelectExited);
    }

    void OnDisable()
    {
        _socket.selectEntered.RemoveListener(OnSelectEntered);
        _socket.selectExited.RemoveListener(OnSelectExited);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        var piece = args.interactableObject.transform.GetComponentInParent<PuzzlePiece>();
        if (!piece) { Reject(args); return; }

        if (piece.pieceId == socketId)
        {
            _current = piece;
            piece.LockInPlace();
            onCorrectPlaced?.Invoke();
        }
        else
        {
            Reject(args);
        }
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        var piece = args.interactableObject.transform.GetComponentInParent<PuzzlePiece>();
        if (piece && piece == _current) _current = null;
    }

    void Reject(SelectEnterEventArgs args)
    {
        onWrongPlaced?.Invoke();

        // force drop
        if (_socket.interactionManager != null)
            _socket.interactionManager.SelectExit(_socket, args.interactableObject);

        // push away slightly
        var rb = args.interactableObject.transform.GetComponentInParent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;
            var dir = (args.interactableObject.transform.position - transform.position).normalized;
            rb.AddForce(dir * rejectForce, ForceMode.VelocityChange);
        }
    }

    public bool IsCorrectlyFilled()
        => _current != null && _current.pieceId == socketId;
}
