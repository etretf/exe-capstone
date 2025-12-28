using UnityEngine;
using static LevelConstants;
using UnityEngine.XR.Interaction.Toolkit;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] GameObject correct_room_prefab;
    [SerializeField] GameObject parasocial_room_prefab;
    [SerializeField] GameObject audio_room_prefab;
    [SerializeField] GameObject surveillance_room_prefab;
    [SerializeField] GameObject recreation_room_prefab;
   
    private GameObject createdRoom;
    public DimensionData dimensionData;

    private void Awake()
    {
        Instance = this;
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CreateRoom()
    {
        RoomType room = TileGenerationManager.Instance.GetRoomTypeAtPlayerLocation();
        TileType tile = TileGenerationManager.Instance.GetTileTypeAtPlayerLocation();
        float z_position = TileGenerationManager.Instance.GetPositionAtPlayerLocation();

        float x_position = -1 * dimensionData.tile_width/2;
        float rotation = 0;

        //adjust position and rotation if the door is on the left
        if(tile == TileType.door_right)
        {
            x_position =  dimensionData.room_width/2 + x_position;
            rotation = 180f; 
        }

        //instatiate room based on room type
        switch (room)
        {
            case RoomType.correct:
                createdRoom = Instantiate(correct_room_prefab);
                break;
            case RoomType.parasocial:
                createdRoom = Instantiate(parasocial_room_prefab);
                break;
            case RoomType.audio:
                createdRoom = Instantiate(audio_room_prefab);
                break;
            case RoomType.surveillance:
                createdRoom = Instantiate(surveillance_room_prefab);
                break;
            case RoomType.recreation:
                createdRoom = Instantiate(recreation_room_prefab);
                break;
            default:
                break;
        }

        createdRoom.transform.position = new Vector3(x_position, 0, z_position);
        createdRoom.transform.Rotate(0, rotation, 0);
    }

    //destroy the current room if it exists 
    public void DestroyRoom()
    {
        if(createdRoom != null)
        {
            Destroy(createdRoom);
        }
    }
}
