using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private DoorLogic doorLogic;
    private ScannerLogic scannerLogic;
    private Transform playerTransform;
    private Transform doorHinge;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeReferences();
    }

    private void InitializeReferences()
    {
        // Find player movement component (likely on the player GameObject)
        playerMovement = GameObject.Find("Player")?.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            // Try alternative names
            playerMovement = FindObjectOfType<PlayerMovement>();
        }

        // Find player transform to reset position
        if (playerMovement != null)
        {
            playerTransform = playerMovement.transform;
        }
        else
        {
            // Fallback: try to find player GameObject directly
            GameObject player = GameObject.Find("Player") ?? GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        // Find door logic
        doorLogic = GameObject.Find("Door")?.GetComponent<DoorLogic>();
        
        // Find door hinge to reset door rotation
        doorHinge = GameObject.Find("Door Hinge")?.GetComponent<Transform>();

        // Find scanner logic
        scannerLogic = GameObject.Find("Scanner")?.GetComponent<ScannerLogic>();
        if (scannerLogic == null)
        {
            scannerLogic = FindObjectOfType<ScannerLogic>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for button 1 or button 3 on Meta controller
        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        Debug.Log("Resetting game...");

        // Reinitialize references if needed
        if (playerTransform == null || doorLogic == null || scannerLogic == null || doorHinge == null)
        {
            InitializeReferences();
        }

        // Reset player position to origin
        if (playerTransform != null)
        {
            playerTransform.position = Vector3.zero;
            playerTransform.rotation = Quaternion.identity;
            Debug.Log("Player reset to origin");
        }

        // Reset door: close it and set required tokens to 100
        if (doorLogic != null)
        {
            doorLogic.setTokens(100);
            doorLogic.setOpenDoor(false);
            doorLogic.showTokenText();
            Debug.Log("Door tokens reset to 100");
        }

        // Close the door by resetting rotation
        if (doorHinge != null)
        {
            doorHinge.rotation = Quaternion.identity;
            Debug.Log("Door closed");
        }

        // Reset scanner tokens to 100
        if (scannerLogic != null)
        {
            scannerLogic.setTokens(100);
            scannerLogic.showTokenText();
            Debug.Log("Scanner tokens reset to 100");
        }
    }
}
