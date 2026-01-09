using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelConstants;

public class TileGenerationManager : MonoBehaviour
{
    public static TileGenerationManager Instance;

    //constants
    const int MAX_TILES = 10;
    const int CORRECT_ROOM_MIN_INDEX = 10;
    const int CORRECT_ROOM_MAX_INDEX = 15;
    public enum TileIndexType
    {
        None,
        Start,
        End
    }

    readonly RoomType[] availableRooms =
    {
        RoomType.audio,
        RoomType.parasocial,
        RoomType.recreation,
        RoomType.surveillance
    };

    //readonly TileLightType[] availableLights =
    //{
    //    TileLightType.incorrect_origin,
    //    TileLightType.incorrect_shadow,
    //    TileLightType.incorrect_temperature
    //};

    //hallway pieces
    [SerializeField] GameObject hallway_tile_left_prefab;
    [SerializeField] GameObject hallway_tile_right_prefab;
    [SerializeField] GameObject hallway_space_tile_prefab;
    [SerializeField] GameObject hallway_end_plane_prefab;

    public DimensionData dimensionData;

    private List<TileData> tiles = new List<TileData>();
    private List<GameObject> active_tiles = new List<GameObject>();

    public int current_tile_index = 0;
    private int correct_room_tile_index = 0;
    private float newest_tile_position = 0;
    private GameObject hallway_end_instance;
    private GameObject hallway_start_instance;
    private TileType last_door_tile_type;
    private RoomType previous_room_type;


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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        correct_room_tile_index = UnityEngine.Random.Range(CORRECT_ROOM_MIN_INDEX, CORRECT_ROOM_MAX_INDEX);

        //generate initial tiles
        hallway_end_instance = Instantiate(hallway_end_plane_prefab);
        hallway_end_instance.transform.position = new Vector3(0, 0, newest_tile_position + 1.5f);
        
        hallway_start_instance = Instantiate(hallway_end_plane_prefab);
        hallway_start_instance.SetActive(false);
        

        tiles.Add(new TileData(TileType.no_door, RoomType.none, TileLightType.none, newest_tile_position, dimensionData.tile_no_door_length));
        AddTileToRender(tiles[0], 0, TileIndexType.End);

        newest_tile_position += (dimensionData.tile_no_door_length / 2f + dimensionData.tile_door_length / 2f);
        tiles.Add(new TileData(TileType.door_left, RoomType.audio, TileLightType.incorrect_temperature, newest_tile_position, dimensionData.tile_door_length));
        last_door_tile_type = TileType.door_left;
        AddTileToRender(tiles[1], 1, TileIndexType.End);
        previous_room_type = RoomType.audio;
        current_tile_index = 1;

        for (int i = 0; i < (MAX_TILES/2)-3; i++) {
            CreateTile();
            current_tile_index = 2+i;
        }
    }

    //create tile
    public void CreateTile()
    {
        if (!tiles.Any()) {
            return;
        }

        TileData current_tile = new TileData();
        TileData previous_tile = tiles.Last();

        //if the previous tile has a door, this one needs a space
        //if the previous tile is just space, this one needs a door
        if (previous_tile.tileType == TileType.no_door)
        {
            current_tile.tileType = last_door_tile_type == TileType.door_left ? TileType.door_right : TileType.door_left;
            last_door_tile_type = current_tile.tileType;
            newest_tile_position += (previous_tile.length / 2f + dimensionData.tile_door_length / 2f);
            current_tile.position = newest_tile_position;
            current_tile.length = dimensionData.tile_door_length;
        } else
        {
            current_tile.tileType = TileType.no_door;
            newest_tile_position += (previous_tile.length / 2f + dimensionData.tile_no_door_length / 2f);
            current_tile.position = newest_tile_position;
            current_tile.length = dimensionData.tile_no_door_length;
        }

        //if this is the nth tile then generate the correct room 
        bool correct_room_created = false;
        if((tiles.Count + 1) % correct_room_tile_index == 0)
        {
            if(current_tile.tileType == TileType.no_door)
            {
                correct_room_tile_index += 1;
            } else
            {
                current_tile.roomType = RoomType.correct;
                current_tile.lightType = TileLightType.normal;
                correct_room_tile_index = UnityEngine.Random.Range(CORRECT_ROOM_MIN_INDEX, CORRECT_ROOM_MAX_INDEX);
                correct_room_created = true;
            }
        }

        //if a correct room was not created then create a incorrect room
        if(!correct_room_created && current_tile.tileType != TileType.no_door)
        {
            //get a random incorrect room
            RoomType roomType = previous_room_type;
            while (roomType == previous_room_type)
            {
                roomType = availableRooms[UnityEngine.Random.Range(0, availableRooms.Length)];
            }
            current_tile.roomType = roomType;
            previous_room_type = roomType;

            //get a random incorrect light
            TileLightType lightType = previous_tile.lightType;
            while (lightType == previous_tile.lightType)
            {
                switch (roomType)
                {
                    case RoomType.audio:
                        lightType = TileLightType.incorrect_temperature;
                        break;
                    case RoomType.parasocial:
                        lightType = TileLightType.incorrect_lamp;
                        break;
                    case RoomType.recreation:
                        lightType = TileLightType.incorrect_radius;
                        break;
                    case RoomType.surveillance:
                        lightType = TileLightType.incorrect_origin;
                        break;
                    default:
                        break;
                }
                //lightType = availableLights[UnityEngine.Random.Range(0, availableLights.Length)];
            }
            current_tile.lightType = lightType;
        }
        
        tiles.Add(current_tile);
        AddTileToRender(current_tile, (tiles.Count-1), TileIndexType.End);
        UpdateHallwayEnds();
    }

    //add a tile prefab to active tiles
    public void AddTileToRender(TileData tile, int tile_index, TileIndexType tile_index_type)
    {
        float extra_tile_width = 0;
        GameObject tile_prefab = null;
        switch (tile.tileType)
        {
            case TileType.no_door:
                tile_prefab = Instantiate(hallway_space_tile_prefab);
                extra_tile_width = dimensionData.tile_no_door_length / 2f;
                break;
            case TileType.door_left:
                tile_prefab = Instantiate(hallway_tile_left_prefab);
                extra_tile_width = dimensionData.tile_door_length / 2f;
                break;
            case TileType.door_right:
                tile_prefab = Instantiate(hallway_tile_right_prefab);
                extra_tile_width = dimensionData.tile_door_length / 2f;
                break;
            default:
                Debug.LogError("Invalid tile type!", this);
                break;
        }

        if (tile_prefab == null) return;

        tile_prefab.transform.position = new Vector3(0, 0, tile.position);
        HallwayTile script = tile_prefab.GetComponent<HallwayTile>();
        script.index = tile_index;

        //temp code. TO REMOVE
        script.temp_room_type = tile.roomType;
        script.temp_light_type = tile.lightType;
        //TO REMOVE - end


        switch (tile_index_type)
        {
            case TileIndexType.Start:
                active_tiles.Insert(0, tile_prefab);
                break;
            case TileIndexType.End:
                active_tiles.Add(tile_prefab);
                break;
            default:
                break;

        }
    }

    public void UpdateHallwayEnds()
    {
        int end_tile_index = active_tiles[active_tiles.Count - 1].GetComponent<HallwayTile>().index;
        int start_tile_index = active_tiles[0].GetComponent<HallwayTile>().index;
        hallway_end_instance.transform.position = new Vector3(0,0, tiles[end_tile_index].position + tiles[end_tile_index].length / 2);
        if (start_tile_index != 0)
        {
            hallway_start_instance.transform.position = new Vector3(0, 0, tiles[start_tile_index].position - tiles[start_tile_index].length / 2);
            hallway_start_instance.SetActive(true);
        } else
        {
            hallway_start_instance.SetActive(false);
        }

    }

    public void UpdateTilesToRender(int tile_index)
    {
        int active_tiles_count = active_tiles.Count;

        //create new tile at the end
        if (!tiles[tile_index].colliderTriggered)
        {
            if (active_tiles_count >= MAX_TILES && tile_index > MAX_TILES / 2) {
                Destroy(active_tiles[0]);
                active_tiles.RemoveAt(0);
            }

            tiles[tile_index].colliderTriggered = true;
            current_tile_index = tile_index;
            CreateTile();
            return;
        }

        //add tile at the beggining
        //remove an active tile from the end and add one to the beggining
        if (tile_index < current_tile_index) {
            
            //remove tile at the end
            if (active_tiles_count > MAX_TILES / 2)
            {
                Destroy(active_tiles[active_tiles.Count - 1]);
                active_tiles.RemoveAt(active_tiles.Count - 1);
            }
            //int tile_to_add_index = tile_index - (MIN_TILES / 2);
            int tile_to_add_index = active_tiles[0].GetComponent<HallwayTile>().index - 1;
            if (tile_to_add_index >= 0)
            {
                TileData tile = tiles[tile_to_add_index];
                AddTileToRender(tile, (tile_to_add_index), TileIndexType.Start);
            }
            UpdateHallwayEnds();
        } 
        //remove a tile from the beggining and add one at the end
        else if (tile_index > current_tile_index)
        {
            //int tile_to_add_index = tile_index + (MIN_TILES / 2);
            int tile_to_add_index = active_tiles[active_tiles.Count-1].GetComponent<HallwayTile>().index + 1;
            if (tile_to_add_index < tiles.Count)
            {
                if (active_tiles_count > MAX_TILES)
                {
                    Destroy(active_tiles[0]);
                    active_tiles.RemoveAt(0);
                }
                TileData tile = tiles[tile_to_add_index];
                AddTileToRender(tile, tile_to_add_index, TileIndexType.End);
            }
            UpdateHallwayEnds();
        }

        current_tile_index = tile_index;
    }

    //private GameObject GetCurrentActiveTileInstance()
    //{
    //    return active_tiles.Find(active_tile => active_tile.GetComponent<HallwayTile>().index == current_tile_index);
    //}

    //helper method to access room type at the current index the player is on
    public RoomType GetRoomTypeAtPlayerLocation()
    {
        return tiles[current_tile_index].roomType;
    }

    //helper method to access tile type at the current tile index the player is on
    public TileType GetTileTypeAtPlayerLocation()
    {
        return tiles[current_tile_index].tileType;
    }

    //helper method to access position at the current tile index that the player is standing on
    public float GetPositionAtPlayerLocation()
    {
        return tiles[current_tile_index].position;
    }

    //helper method to access light type at the current index the player is on
    public TileLightType GetLightTypeAtIndex(int tile_index)
    {
        return tiles[tile_index].lightType;
    }

    //helper to return the door that belongs to the current tile
    public HallwayDoor GetDoorOfCurrentTile()
    {
        GameObject current_tile = active_tiles.Find(active_tile => active_tile.GetComponent<HallwayTile>().index == current_tile_index);
        return current_tile.GetComponentInChildren<HallwayDoor>();
    }

    //helper to return the door that belongs to a specific active tile
    public HallwayDoor GetDoorAtIndex(int tile_index)
    {
        GameObject tile_at_index = active_tiles.Find(active_tile => active_tile.GetComponent<HallwayTile>().index == tile_index);
        return tile_at_index.GetComponentInChildren<HallwayDoor>();
    }

}
