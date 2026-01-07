using UnityEngine;

[CreateAssetMenu(fileName = "LightData", menuName = "Scriptable Objects/LightData")]
public class LightData : ScriptableObject
{
    public float wrong_origin_z_offset = 0;
    public float wrong_origin_intensity = 0;
    public float small_radius_range = 0;
    public float small_radius_intensity = 0;
    public byte wrong_colour_red = 0;
    public byte wrong_colour_green = 0;
    public byte wrong_colour_blue = 0;
    public Color wrong_colour_emission_colour = Color.white;
}
