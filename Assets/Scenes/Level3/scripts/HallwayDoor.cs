
public class HallwayDoor : Door
{
    //show door instructions
    public override void DisplayDoorControls()
    {
        if(is_door_closed && GameManager.Instance.GetIsPlayerInHallway())
        {
            control_instructions.gameObject.SetActive(true);
        }
    }
}
