using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScannerLogic : MonoBehaviour
{

    public float triggerThreshold = 0.5f;
    public TextMeshPro tokenText;
    public int tokens = 0;

    private OVRCameraRig playerCamera;
    private string currentScannerPosition = "right";


    void Start()
    {
        try {
        toggleScannerVisibility(false);
            this.playerCamera = GameObject.Find("VR Headset").GetComponent<OVRCameraRig>();
            this.tokenText = GameObject.Find("Scanner Token Text").GetComponent<TextMeshPro>();
            
            setTokenTextPosition();
        } catch (System.Exception e) {
            Debug.LogError("Error in initializing ScannerLogic: " + e.Message);
        }
    }
    
    void Update()
    {
        try {
            if(leftToggleTrigger()){
                toggleScannerVisibility(true);
                currentScannerPosition = "left";
            }else if(rightToggleTrigger()){
                toggleScannerVisibility(true);
                currentScannerPosition = "right";
            } else {
                toggleScannerVisibility(false);
            }
            setScannerPos(currentScannerPosition);
        } catch (System.Exception e) {
            Debug.LogError("Error in ScannerLogic: " + e.Message);
        }
    }

    public void toggleScannerVisibility(bool showScanner) {
        try {
            GetComponent<Renderer>().enabled = showScanner;
            this.tokenText.gameObject.GetComponent<Renderer>().enabled = showScanner;
        } catch (System.Exception e) {
            Debug.LogError("Error when toggling scanner visibility: " + e.Message);
        }
    }

    public void setScannerPos(string currentScannerPosition){
        if(currentScannerPosition == "left"){
            transform.SetParent(playerCamera.leftHandAnchor, false);
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.identity;
            transform.Rotate(0, 90, 0);
        }else if(currentScannerPosition == "right"){
            transform.SetParent(playerCamera.rightHandAnchor, false);
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.identity;
            transform.Rotate(0, 90, 0);
        }
    }

    public void setTokenTextPosition(){
        // this.tokenText.autoSizeTextContainer = true;
        // this.tokenText.enableAutoSizing = true;
        this.tokenText.transform.SetParent(transform, false);
        this.tokenText.transform.localScale = Vector3.one;
        this.tokenText.transform.position = transform.position + new Vector3(0, 0.1f, 0);
        this.tokenText.transform.rotation = transform.rotation;
        this.tokenText.transform.Rotate(0, -90, 0);
        this.tokenText.fontSize = 10f;
        this.tokenText.autoSizeTextContainer = true;
        
        this.tokenText.text = "Tokens: " + tokens.ToString();
    }

    public bool leftToggleTrigger() => OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > triggerThreshold;
    public bool rightToggleTrigger() => OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > triggerThreshold;
    
    public Vector3 rightControllerPosition() => playerCamera.trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
    public Quaternion rightControllerRotation() => OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    public Vector3 leftControllerPosition() => playerCamera.trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
    public Quaternion leftControllerRotation() => OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
}
