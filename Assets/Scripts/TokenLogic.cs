using UnityEngine;

public class TokenLogic : MonoBehaviour
{
    private DoorLogic doorLogic;
    private ScannerLogic scannerLogic;
    private float elapsedTime = 0f;
    private float tokenTransferTime = 0.2f;
    private bool finishedTransfer = false;


    void Start()
    {
        this.doorLogic = GameObject.Find("Door").GetComponent<DoorLogic>();
        this.scannerLogic = GameObject.Find("Scanner").GetComponent<ScannerLogic>();
    }

   
    void Update()
    {
        if (isScannerNearDoor() && this.scannerLogic.isScannerVisible() && !this.finishedTransfer){
            elapsedTime += Time.deltaTime;
            if (elapsedTime <= tokenTransferTime) return;
            this.scannerLogic.setTokens(this.scannerLogic.getTokens() - 1);
            this.doorLogic.setTokens(this.doorLogic.getTokens() - 1);
        
            this.scannerLogic.showTokenText();
            this.doorLogic.showTokenText();
            elapsedTime = 0f;
        }
        
        if (this.doorLogic.getTokens() == 0){
            this.doorLogic.setOpenDoor(true);
            this.finishedTransfer = true;
        }
    }

    private bool isScannerNearDoor() {
        Bounds doorBounds = this.doorLogic.getDoorCollider().bounds;
        Bounds scannerBounds = this.scannerLogic.getScannerCollider().bounds;
        return doorBounds.Intersects(scannerBounds) || (doorBounds.Contains(scannerBounds.min) && doorBounds.Contains(scannerBounds.max));
    }

    private bool isScannerTriggered() => this.scannerLogic.isLeftToggleTriggered() || this.scannerLogic.isRightToggleTriggered();
    
}
