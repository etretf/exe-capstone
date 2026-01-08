using UnityEngine;

//contains constants used throughout the level 3 scripts
public static class LevelConstants
{
    public enum TileType
    {
        none,
        door_left,
        door_right,
        no_door
    }
    public enum RoomType
    {
        none,
        correct,
        audio,
        parasocial,
        surveillance,
        recreation
    }
    public enum TileLightType
    {
        none,
        normal, 
        incorrect_temperature,
        incorrect_origin,
        incorrect_radius,
        incorrect_lamp,
    }

    public const int HINT_TEXT_NTH_TILE = 4;
}
