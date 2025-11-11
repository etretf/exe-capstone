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
        toggleScannerVisibility(false);
        this.playerCamera = GameObject.Find("VR Headset").GetComponent<OVRCameraRig>();
        this.tokenText = GameObject.Find("Token Text").GetComponent<TextMeshPro>();
        setTokenTextPosition();
        
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
            transform.position = leftControllerPosition();
            transform.rotation = leftControllerRotation();
            transform.Rotate(0, 90, 0);
        }else if(currentScannerPosition == "right"){
            transform.position = rightControllerPosition();
            transform.rotation = rightControllerRotation();
            transform.Rotate(0, 90, 0);
        }
    }

    public void setTokenTextPosition(){
        this.tokenText.transform.SetParent(transform);
        this.tokenText.transform.position = transform.position + new Vector3(0, 0.1f, 0);
        this.tokenText.transform.rotation = transform.rotation;
        this.tokenText.transform.Rotate(0, -90, 0);
        this.tokenText.text = "Tokens: " + tokens.ToString();
    }

    public bool leftToggleTrigger() => OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > triggerThreshold;
    public bool rightToggleTrigger() => OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > triggerThreshold;
    
    public Vector3 rightControllerPosition() => playerCamera.trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
    public Quaternion rightControllerRotation() => OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    public Vector3 leftControllerPosition() => playerCamera.trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
    public Quaternion leftControllerRotation() => OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
}
