using UnityEngine;

//class contains info on tile
public class TileData
{
    // Fields
    public LevelConstants.TileType tileType;
    public LevelConstants.RoomType roomType;
    public LevelConstants.TileLightType lightType;
    public bool lightTriggered;

    public TileData()
    {
    }

    public TileData(LevelConstants.TileType tileType, LevelConstants.RoomType roomType, LevelConstants.TileLightType lightType)
    {
        this.tileType = tileType;
        this.roomType = roomType;
        this.lightType = lightType;
        this.lightTriggered = false;
    }
}