using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScannerLogic : MonoBehaviour
{

    private float triggerThreshold = 0.5f;
    
    private int tokens = 100;

    private OVRCameraRig playerCamera;
    private enum scannerPosition { left, right };
    private scannerPosition currentScannerPosition = scannerPosition.right;
    private TextMeshPro tokenText;

    float elapsedTime = 0f;
    float transferInterval = 0.05f;

    private ParticleSystem tokenTransferParticles;
    private DoorLogic doorLogic;


    void Start()
    {
        Debug.Log("ScannerLogic started");
        try {
            this.toggleScannerVisibility(false);
            initializePlayerCamera();
            initializeTokenText();
            initializeDoorLogic();
            initializeTokenTransferParticles();
        } catch (System.Exception e) {
            Debug.LogError("Error in initializing ScannerLogic: " + e.Message);
        }
    }
    
    void Update()
    {
        try {
            if (playerCamera == null) initializePlayerCamera();
            if (tokenText == null) initializeTokenText();

            if(this.isLeftToggleTriggered()){
                toggleScannerVisibility(true);
                currentScannerPosition = scannerPosition.left;
            }else if(this.isRightToggleTriggered()){
                toggleScannerVisibility(true);
                currentScannerPosition = scannerPosition.right;
            } else {
                toggleScannerVisibility(false);
            }
            this.setScannerPosition();

            if (this.isScannerNearDoor() && this.isScannerVisible() && this.getTokens() > 0) {
                if (elapsedTime >= transferInterval) {
                    this.transferTokens();
                    elapsedTime = 0f;
                }
                elapsedTime += Time.deltaTime;
            }

            this.updateParticleDirectionToDoor();
        } catch (System.Exception e) {
            Debug.LogError("Error in ScannerLogic: " + e.Message);
        }
    }

    // Update particle direction to door each frame (makes particles track moving door)
    private void updateParticleDirectionToDoor(){
        try {
            // Only update if particle system exists and has active particles
            if (tokenTransferParticles == null || doorLogic == null) {
                return;
            }
            
            // Check if there are any active particles
            if (!tokenTransferParticles.isPlaying && tokenTransferParticles.particleCount == 0) {
                return;
            }
            
            // Calculate current direction to door from scanner position
            Vector3 directionToDoor = (doorLogic.getDoorPosition() - transform.position).normalized;
            
            // Update velocity over lifetime to point toward current door position
            var velocityOverLifetime = tokenTransferParticles.velocityOverLifetime;
            if (velocityOverLifetime.enabled) {
                float particleSpeed = 5f; // Adjust speed as needed
                
                // Update velocity direction each frame
                velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(directionToDoor.x * particleSpeed);
                velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(directionToDoor.y * particleSpeed);
                velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(directionToDoor.z * particleSpeed);
            }
        } catch (System.Exception e) {
            // Silently fail - don't spam errors if door or particles aren't ready
            // Debug.LogWarning("Error updating particle direction: " + e.Message);
        }
    }

    private void toggleScannerVisibility(bool showScanner) {
        GetComponent<Renderer>().enabled = showScanner;
        this.tokenText.gameObject.GetComponent<Renderer>().enabled = showScanner;
    }

    private void setScannerPosition(){
        try {
            if(this.currentScannerPosition == scannerPosition.left){
                this.transform.SetParent(playerCamera.leftHandAnchor, false);
            } else {
                this.transform.SetParent(playerCamera.rightHandAnchor, false);
            }
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            this.transform.Rotate(0, 90, 0);
        } catch (System.Exception e) {
            Debug.LogError("Error in setting scanner position: " + e.Message);
        }
    }

    private void setTokenText(){ 
        this.tokenText.transform.SetParent(transform, false);
        this.tokenText.transform.localScale = Vector3.one;
        this.tokenText.transform.position = transform.position + new Vector3(0, 0.1f, 0);
        this.tokenText.transform.rotation = transform.rotation;
        this.tokenText.transform.Rotate(0, -90, 0);
        this.tokenText.fontSize = 10f;
        this.tokenText.autoSizeTextContainer = true;
        
        this.showTokenText();
    }

    private void initializeTokenText(){
        this.tokenText = GameObject.Find("Scanner Token Text")?.GetComponent<TextMeshPro>();
        this.setTokenText();
    }

    private void initializePlayerCamera(){
        this.playerCamera = GameObject.Find("VR Headset")?.GetComponent<OVRCameraRig>();
    }

    private void initializeDoorLogic(){
        this.doorLogic = GameObject.Find("Door")?.GetComponent<DoorLogic>();
    }

    private void initializeTokenTransferParticles(){
        this.tokenTransferParticles = GetComponent<ParticleSystem>();
    }

    public void transferTokens(){
        try {

            if (doorLogic == null) initializeDoorLogic();
            if (this.tokenTransferParticles == null) initializeTokenTransferParticles();

            
            Vector3 directionToDoor = (doorLogic.getDoorPosition() - transform.position).normalized;
            var velocityOverLifetime = this.tokenTransferParticles.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.World;

            // Configure main module first - CRITICAL for proper velocity control
            var main = tokenTransferParticles.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World; // Use world space
            main.startSpeed = 0f; // CRITICAL: Set to 0 so velocity over lifetime controls all movement
            main.startLifetime = 5f; // Particles live for 5 seconds
            main.gravityModifier = 0f; // No gravity

            // Set velocity using MinMaxCurve with the direction vector multiplied by speed
            float particleSpeed = 1f; // Change this value to change speed, NOT direction
            velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(directionToDoor.x * particleSpeed);
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(directionToDoor.y * particleSpeed);
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(directionToDoor.z * particleSpeed);


            this.tokenTransferParticles.Emit(1);
            this.setTokens(this.getTokens() - 1);
            this.showTokenText();
        } catch (System.Exception e) {
            Debug.LogError("Error in emitting token transfer particles: " + e.Message);
        }
    }

    private bool isScannerNearDoor() {
        if (this.doorLogic == null) initializeDoorLogic();
        Bounds doorBounds = this.doorLogic.getDoorCollider().bounds;
        Bounds scannerBounds = this.getScannerCollider().bounds;
        return doorBounds.Intersects(scannerBounds) || (doorBounds.Contains(scannerBounds.min) && doorBounds.Contains(scannerBounds.max));
    }

    public BoxCollider getScannerCollider() => GetComponent<BoxCollider>();
    public int getTokens() => tokens;
    public void setTokens(int tokens) => this.tokens = tokens;

    public void showTokenText() => this.tokenText.text = "Tokens: " + tokens.ToString();

    public bool isScannerVisible() => GetComponent<Renderer>().enabled;
    public bool isLeftToggleTriggered() => OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > triggerThreshold;
    public bool isRightToggleTriggered() => OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > triggerThreshold;
}
