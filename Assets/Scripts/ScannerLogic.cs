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

    void Start()
    {
        Debug.Log("ScannerLogic started");
        try {
            this.toggleScannerVisibility(false);
            initializePlayerCamera();
            initializeTokenText();
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
        } catch (System.Exception e) {
            Debug.LogError("Error in ScannerLogic: " + e.Message);
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

    public BoxCollider getScannerCollider() => GetComponent<BoxCollider>();
    public int getTokens() => tokens;
    public void setTokens(int tokens) => this.tokens = tokens;

    public void showTokenText() => this.tokenText.text = "Tokens: " + tokens.ToString();

    public bool isScannerVisible() => GetComponent<Renderer>().enabled;
    public bool isLeftToggleTriggered() => OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > triggerThreshold;
    public bool isRightToggleTriggered() => OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > triggerThreshold;
}
