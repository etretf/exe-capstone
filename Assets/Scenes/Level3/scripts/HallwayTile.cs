using UnityEngine;


public class HallwayTile : MonoBehaviour
{
    public int index;
    [SerializeField] Light tile_light;
    private Door door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        door = gameObject.GetComponentInChildren<Door>();
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (tile_light != null)
        {
            tile_light.enabled = true;
        }

        GameManager.Instance.SetPlayerInHallway();

        int previous_tile_index = TileGenerationManager.Instance.current_tile_index;
        if (index != previous_tile_index)
        {
            //get previous tile door before doing everything else
            TileGenerationManager.Instance.UpdateTilesToRender(index);
            if (RoomManager.Instance.DoesRoomExist())
            {
                TileGenerationManager.Instance.GetDoorAtIndex(previous_tile_index).CloseDoor();
                RoomManager.Instance.DestroyRoom();
            }
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (tile_light != null)
        {
            tile_light.enabled = false;
        }
        
    }
}
