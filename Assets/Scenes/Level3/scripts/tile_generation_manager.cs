using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelConstants;

public class TileGenerationManager : MonoBehaviour
{
    public static TileGenerationManager Instance;

    //constants
    const int MIN_TILES = 10;
    public enum TileIndexType
    {
        None,
        Start,
        End
    }
    
    //hallway pieces
    [SerializeField] GameObject hallway_tile_left_prefab;
    [SerializeField] GameObject hallway_tile_right_prefab;
    [SerializeField] GameObject hallway_space_tile_prefab;

    //hallway pieces length
    [SerializeField] float hallway_tile_left_prefab_length;
    [SerializeField] float hallway_tile_right_prefab_length;
    [SerializeField] float hallway_space_tile_prefab_length;

    private List<TileData> tiles = new List<TileData>();
    private List<GameObject> active_tiles = new List<GameObject>();

    private int correct_rooms_count = 0;
    private int current_tile_index = 0;
    private float newest_tile_position = 0;

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
        tiles.Add(new TileData(TileType.no_door, RoomType.none, TileLightType.none, newest_tile_position, hallway_space_tile_prefab_length));
        AddTileToRender(tiles[0], 0, TileIndexType.End);

        newest_tile_position += (hallway_space_tile_prefab_length / 2 + hallway_tile_left_prefab_length / 2);
        tiles.Add(new TileData(TileType.door_left, RoomType.audio, TileLightType.incorrect_shadow, newest_tile_position, hallway_tile_left_prefab_length));
        AddTileToRender(tiles[1], 1, TileIndexType.End);

        for (int i = 0; i < MIN_TILES-3; i++) {
            CreateTile();
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
            //TODO: implement enum randomaization so we can select random door
            current_tile.tileType = TileType.door_right;
            newest_tile_position += (previous_tile.length / 2 + hallway_tile_right_prefab_length / 2);
            current_tile.position = newest_tile_position;
            current_tile.length = hallway_tile_right_prefab_length;
        } else
        {
            current_tile.tileType = TileType.no_door;
            newest_tile_position += (previous_tile.length / 2 + hallway_space_tile_prefab_length / 2);
            current_tile.position = newest_tile_position;
            current_tile.length = hallway_tile_left_prefab_length;
        }

        //TODO: randomize enum
        current_tile.roomType = RoomType.audio;
    
        //TODO: randomize enum
        current_tile.lightType = TileLightType.incorrect_shadow;

        tiles.Add(current_tile);
        AddTileToRender(current_tile, (tiles.Count-1), TileIndexType.End);
    }

    //add a tile prefab to active tiles
    public void AddTileToRender(TileData tile, int tile_index, TileIndexType tile_index_type)
    {
        GameObject tile_prefab = null;
        switch (tile.tileType)
        {
            case TileType.no_door:
                tile_prefab = Instantiate(hallway_space_tile_prefab);
                break;
            case TileType.door_left:
                tile_prefab = Instantiate(hallway_tile_left_prefab);
                break;
            case TileType.door_right:
                tile_prefab = Instantiate(hallway_tile_right_prefab);
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
                break;
            case TileIndexType.End:
                active_tiles.Add(tile_prefab);
                break;
            default:
                break;

        }

        current_tile_index = tile_index;
    }

    public void UpdateTilesToRender(int tile_index)
    {
        int active_tiles_count = active_tiles.Count;

        //create new tile at the end
        if (!tiles[tile_index].colliderTriggered)
        {
            if (active_tiles_count >= MIN_TILES && tile_index > MIN_TILES / 2) {
                Destroy(active_tiles[0]);
                active_tiles.RemoveAt(0);
            }
            
            CreateTile();
            tiles[tile_index].colliderTriggered = true;
            return;
        }

        //add tile at the beggining
        //remove an active tile from the end and add one to the beggining
        if (tile_index < current_tile_index && active_tiles_count >= MIN_TILES) {
            Destroy(active_tiles[active_tiles.Count - 1]);
            active_tiles.RemoveAt(active_tiles.Count - 1);
            TileData tile = tiles[tile_index - (MIN_TILES / 2)];
            AddTileToRender(tile, (tile_index - (MIN_TILES / 2)), TileIndexType.Start);
        } 
        //remove a tile from the beggining and add one at the end
        else if (tile_index > current_tile_index && active_tiles_count >= MIN_TILES)
        {
            Destroy(active_tiles[0]);
            active_tiles.RemoveAt(0);
            TileData tile = tiles[tile_index + (MIN_TILES / 2)];
            AddTileToRender(tile, (tile_index - (MIN_TILES / 2)), TileIndexType.End);
        }
    }

}
