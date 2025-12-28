using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/DimensionData")]
public class DimensionData : ScriptableObject
{
    public float tile_door_length;
    public float tile_width;
    public float tile_no_door_length;

    public float room_width;
}
