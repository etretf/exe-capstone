using UnityEngine;

[CreateAssetMenu(fileName = "LevelDelays", menuName = "Scriptable Objects/LevelDelays")]
public class LevelDelays : ScriptableObject
{
    public float door_open_delay_seconds = 0;
    public float pause_between_phone_rings = 0;
}
