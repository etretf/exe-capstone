using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float constantMoveSpeed = 5f;
    public float constantRotationSpeed = 180f;

    private float rightStickDeadZone = 0.1f;

    public void Start()
    {
    }

    public void Update()
    {
        if(!playerCamera().isActiveAndEnabled) 
        {
            Debug.LogError("Player camera is not active and enabled");
            return;
        }

        rotatePlayer();
        transform.position += (moveDirection() * moveSpeed() * Time.deltaTime);
    }

    public Camera playerCamera() => Camera.main;

    public Vector3 moveDirection(){
        Vector3 forward = playerCamera().transform.forward;
        Vector3 right = playerCamera().transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        return (forward * primaryThumbstickDirection().y + right * primaryThumbstickDirection().x).normalized;
    }

    public void rotatePlayer()
    {
        if(Mathf.Abs(secondaryThumbstickDirection().x) < rightStickDeadZone) return;

        float yawDelta = secondaryThumbstickDirection().x * constantRotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, yawDelta, Space.World);
    }

    public float moveSpeed() => primaryThumbstickDirection().magnitude * constantMoveSpeed;

    public Vector2 primaryThumbstickDirection() => OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

    public Vector2 secondaryThumbstickDirection() => OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

}
