using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorLogic : MonoBehaviour
{
    public int numTokensRequired = 1;
    
    private TextMeshPro doorText;

    void Start()
    {
        try {
            this.doorText = GameObject.Find("Door Token Text").GetComponent<TextMeshPro>();
            
            setDoorTextPosition();
        } catch (System.Exception e) {
            Debug.LogError("Error in initializing DoorLogic: " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setDoorTextPosition(){
        
        // this.doorText.enableAutoSizing = true;
        this.doorText.transform.SetParent(transform, false);
        this.doorText.transform.position = transform.position + new Vector3(-0.2f, 0, 0);
        this.doorText.transform.rotation = transform.rotation;
        this.doorText.transform.Rotate(0, 90, 0);
        this.doorText.fontSize = 1f;
        this.doorText.autoSizeTextContainer = true;
        this.doorText.text = "Tokens: " + numTokensRequired.ToString();
    }
}
