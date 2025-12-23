using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelConstants;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //constants
    const int MIN_TILES = 10;
    
    //hallway pieces
    [SerializeField] GameObject hallway_tile_left_prefab;
    [SerializeField] GameObject hallway_tile_right_prefab;
    [SerializeField] GameObject hallway_space_tile_prefab;

    //stores generated hallway pieces
    private List<TileData> tiles = new List<TileData>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //generate initial tiles
        tiles.Add(new TileData(TileType.no_door, RoomType.none, TileLightType.none));
        tiles.Add(new TileData(TileType.door_left, RoomType.audio, TileLightType.incorrect_shadow));
        for (int i = 0; i < MIN_TILES-2; i++) {
            createTile();
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //create tile
    public void createTile()
    {
        if (!tiles.Any()) {
            return;
        }

        TileData current_tile = new TileData();
        TileData prevTile = tiles.Last();

        //if the previous tile has a door, this one needs a space
        //if the previous tile is just space, this one needs a door
        if (prevTile.tileType == TileType.no_door)
        {
            //TO-DO: implement enum randomaization so we can select random door
            current_tile.tileType = TileType.door_right;
        }

        current_tile.roomType = RoomType.audio;

        current_tile.lightType = TileLightType.incorrect_shadow;

        tiles.Add(current_tile);
    }


    //render tiles
}
