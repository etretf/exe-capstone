using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorLogic : MonoBehaviour
{
    private int numTokensRequired = 100;
    private bool openDoor = false;
    private Transform doorHinge;
    private TextMeshPro doorText;

    void Start()
    {
        try {
            this.doorText = GameObject.Find("Door Token Text").GetComponent<TextMeshPro>();
            this.doorHinge = GameObject.Find("Door Hinge").GetComponent<Transform>();
            transform.SetParent(this.doorHinge);
            this.setDoorText();
        } catch (System.Exception e) {
            Debug.LogError("Error in initializing DoorLogic: " + e.Message);
        }
    }

    void Update(){
        if (this.openDoor) this.rotateDoor();
    }

    private void setDoorText(){
        this.doorText.transform.SetParent(transform, false);
        this.doorText.transform.position = transform.position + new Vector3(-0.2f, 0, 0);
        this.doorText.transform.rotation = transform.rotation;
        this.doorText.transform.Rotate(0, 90, 0);
        this.doorText.fontSize = 1f;
        this.doorText.autoSizeTextContainer = true;

        this.showTokenText();
    }

    private void rotateDoor(){
        this.doorHinge.rotation = Quaternion.Slerp(this.doorHinge.rotation, Quaternion.Euler(0, -90, 0), 1f * Time.deltaTime);
        if (this.doorHinge.rotation.eulerAngles.y <= -90) this.openDoor = false;
    }

    public bool getOpenDoor() => this.openDoor;
    public void setOpenDoor(bool openDoor) => this.openDoor = openDoor;

    public BoxCollider getDoorCollider() => GameObject.Find("Door Detector").GetComponent<BoxCollider>();

    public int getTokens() => this.numTokensRequired;
    public void setTokens(int numTokensRequired) => this.numTokensRequired = numTokensRequired;

    public void showTokenText() => this.doorText.text = "Tokens Required: " + numTokensRequired.ToString();
}
