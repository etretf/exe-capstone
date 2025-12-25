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

    //hallway pieces
    [SerializeField] GameObject hallway_tile_left_prefab;
    [SerializeField] GameObject hallway_tile_right_prefab;
    [SerializeField] GameObject hallway_space_tile_prefab;
    [SerializeField] GameObject hallway_end_plane_prefab;

    //hallway pieces length
    [SerializeField] float hallway_tile_left_prefab_length;
    [SerializeField] float hallway_tile_right_prefab_length;
    [SerializeField] float hallway_space_tile_prefab_length;

    private List<TileData> tiles = new List<TileData>();
    private List<GameObject> active_tiles = new List<GameObject>();

    private int correct_rooms_count = 0;
    private int current_tile_index = 0;
    private float newest_tile_position = 0;
    private GameObject hallway_end_instance;
    private GameObject hallway_start_instance;
    private TileType last_door_tile_type;

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

        //generate initial tiles
        hallway_end_instance = Instantiate(hallway_end_plane_prefab);
        hallway_end_instance.transform.position = new Vector3(0, 0, newest_tile_position + 1.5f);
        
        hallway_start_instance = Instantiate(hallway_end_plane_prefab);
        hallway_start_instance.SetActive(false);
        

        tiles.Add(new TileData(TileType.no_door, RoomType.none, TileLightType.none, newest_tile_position, hallway_space_tile_prefab_length));
        AddTileToRender(tiles[0], 0, TileIndexType.End);

        newest_tile_position += (hallway_space_tile_prefab_length / 2f + hallway_tile_left_prefab_length / 2f);
        tiles.Add(new TileData(TileType.door_left, RoomType.audio, TileLightType.incorrect_shadow, newest_tile_position, hallway_tile_left_prefab_length));
        last_door_tile_type = TileType.door_left;
        AddTileToRender(tiles[1], 1, TileIndexType.End);
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
            newest_tile_position += (previous_tile.length / 2f + hallway_tile_right_prefab_length / 2f);
            current_tile.position = newest_tile_position;
            current_tile.length = hallway_tile_right_prefab_length;
        } else
        {
            current_tile.tileType = TileType.no_door;
            newest_tile_position += (previous_tile.length / 2f + hallway_space_tile_prefab_length / 2f);
            current_tile.position = newest_tile_position;
            current_tile.length = hallway_space_tile_prefab_length;
        }

        //TODO: randomize enum
        RoomType roomType = previous_tile.roomType;
        while (roomType == previous_tile.roomType)
        {
            roomType = availableRooms[UnityEngine.Random.Range(0, availableRooms.Length)];
        }
        current_tile.roomType = roomType;

        //TODO: randomize enum
        current_tile.lightType = TileLightType.incorrect_shadow;

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
                extra_tile_width = hallway_space_tile_prefab_length / 2f;
                break;
            case TileType.door_left:
                tile_prefab = Instantiate(hallway_tile_left_prefab);
                extra_tile_width = hallway_tile_left_prefab_length / 2f;
                break;
            case TileType.door_right:
                tile_prefab = Instantiate(hallway_tile_right_prefab);
                extra_tile_width = hallway_tile_right_prefab_length / 2f;
                break;
            default:
                Debug.LogError("Invalid tile type!", this);
                break;
        }

        if (tile_prefab == null) return;

        tile_prefab.transform.position = new Vector3(0, 0, tile.position);
        HallwayTile script = tile_prefab.GetComponent<HallwayTile>();
        script.index = tile_index;


        switch (tile_index_type)
        {
            case TileIndexType.Start:
                active_tiles.Insert(0, tile_prefab);
                ////move hallway closing planes
                //int last_tile_index = active_tiles[active_tiles.Count-1].GetComponent<HallwayTile>().index;
                //hallway_end_instance.transform.position = new Vector3(0, 0, tiles[last_tile_index].position - tiles[last_tile_index].length / 2f);
                //if (tile_index != 0)
                //{
                //    hallway_start_instance.SetActive(true);
                //    hallway_start_instance.transform.position = new Vector3(0, 0, tile.position - extra_tile_width);
                //} else
                //{
                //    hallway_start_instance.SetActive(false);
                //}
                break;
            case TileIndexType.End:
                active_tiles.Add(tile_prefab);
                ////move hallway closing planes
                //hallway_end_instance.transform.position = new Vector3(0, 0, tile.position + extra_tile_width);

                //int first_tile_index = active_tiles[0].GetComponent<HallwayTile>().index;
                //if (first_tile_index != 0)
                //{
                //    hallway_start_instance.SetActive(true);
                //    hallway_start_instance.transform.position = new Vector3(0, 0, tiles[first_tile_index].position - tiles[first_tile_index].length/2f);
                //} else
                //{
                //    hallway_start_instance.SetActive(false);
                //}
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

}
