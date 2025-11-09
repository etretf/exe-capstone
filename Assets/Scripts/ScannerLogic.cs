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
    private bool isTriggerDown = false;
    private bool isScannerVisible = false;


    void Start()
    {
        playerCamera = GameObject.Find("VR Headset").GetComponent<OVRCameraRig>();
        tokenText = GameObject.Find("Token Text").GetComponent<TextMeshPro>();
        tokenText.transform.SetParent(transform);
        tokenText.transform.position = transform.position + new Vector3(0, 0.1f, 0);
        tokenText.transform.rotation = transform.rotation;
        tokenText.transform.Rotate(0, -90, 0);
        tokenText.text = "Tokens: " + tokens.ToString();
    }

    
    void Update()
    {
        if(leftToggleTrigger()){
            if(currentScannerPosition != "right" || !isScannerVisible){
                isTriggerDown = true;
            }
            currentScannerPosition = "left";
        }else if(rightToggleTrigger()){
            if(currentScannerPosition != "left" || !isScannerVisible){
                isTriggerDown = true;
            }
            currentScannerPosition = "right";
        }
        if(isTriggerDown){
            toggleScannerVisibility();
            isTriggerDown = false;
        }
        setScannerPos(currentScannerPosition);
        setTokenTextPosition();
    }

    public void toggleScannerVisibility() {
        GetComponent<Renderer>().enabled = !isScannerVisible;
        isScannerVisible = !isScannerVisible;
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
        // tokenText.transform.localRotation= transform.rotation;
        // tokenText.transform.Rotate(0, -90, 0);
        // tokenText.transform.localPosition = transform.position + new Vector3(0, 0.1f, 0);
        
        
    }

    public bool leftToggleTrigger() => OVRInput.GetDown(OVRInput.Button.Three);
    public bool rightToggleTrigger() => OVRInput.GetDown(OVRInput.Button.One);
    public Vector3 rightControllerPosition() => playerCamera.trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
    public Quaternion rightControllerRotation() => OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    public Vector3 leftControllerPosition() => playerCamera.trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
    public Quaternion leftControllerRotation() => OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
}
