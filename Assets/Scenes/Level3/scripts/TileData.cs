using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//class contains info on tile
public class TileData
{
    // Fields
    public LevelConstants.TileType tileType;
    public LevelConstants.RoomType roomType;
    public LevelConstants.TileLightType lightType;
    public float position;
    public float length;
    public bool colliderTriggered;

    public TileData()
    {
    }

    public TileData(LevelConstants.TileType tileType, LevelConstants.RoomType roomType, LevelConstants.TileLightType lightType, float position, float length)
    {
        this.tileType = tileType;
        this.roomType = roomType;
        this.lightType = lightType;
        this.colliderTriggered = false;
        this.position = position;
        this.length = length;
    }
}