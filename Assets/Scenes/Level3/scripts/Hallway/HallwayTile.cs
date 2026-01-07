using System.Collections;
using System.Drawing;
using UnityEngine;
using static LevelConstants;


public class HallwayTile : MonoBehaviour
{
    public int index;
    [SerializeField] Light tile_light;
    [SerializeField] GameObject hall_tile_model;
    [SerializeField] int emission_mat_id;
    [SerializeField] LightData light_data;

    private Door door;
    public AudioSource light_audio_src;
    public AudioClip door_delay_sfx;
    public AudioClip door_open_sfx;
    public AudioClip door_close_sfx;
    public AudioClip light_hum_start_sfx;
    public AudioClip[] light_hum_clips_sfx;
    private TileLightType light_type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        door = gameObject.GetComponentInChildren<Door>();
        if (hall_tile_model != null)
            hall_tile_model.GetComponent<MeshRenderer>().materials[emission_mat_id].DisableKeyword("_EMISSION");

        light_type = TileGenerationManager.Instance.GetLightTypeAtIndex(index);
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        GameManager.Instance.SetPlayerInHallway();

        int previous_tile_index = TileGenerationManager.Instance.current_tile_index;
        if (index != previous_tile_index)
        {
            //get previous tile door before doing everything else
            TileGenerationManager.Instance.UpdateTilesToRender(index);
            if (RoomManager.Instance.DoesRoomExist())
            {
                TileGenerationManager.Instance.GetDoorAtIndex(previous_tile_index).CloseDoor();
                RoomManager.Instance.DestroyRoom();
            }
        }

        // Trigger sounds and turn on light
        if (light_audio_src != null)
        {
            StartCoroutine(playLightSound());
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (tile_light != null)
        {
            tile_light.enabled = false;
        }

        if (hall_tile_model != null)
            hall_tile_model.GetComponent<MeshRenderer>().materials[emission_mat_id].DisableKeyword("_EMISSION");

        if (light_audio_src != null)
        {
            light_audio_src.Stop();
        }
    }

    IEnumerator playLightSound()
    {
        light_audio_src.clip = light_hum_start_sfx;
        light_audio_src.Play();

        if (tile_light != null)
        {
            tile_light.enabled = true;
        }

        if (hall_tile_model != null)
            UpdateLight();

        yield return new WaitForSeconds(light_hum_start_sfx.length);

        light_audio_src.clip = light_hum_clips_sfx[Random.Range(0, light_hum_clips_sfx.Length)];
        light_audio_src.loop = true;
        light_audio_src.Play();
    }

    //function updates the light characteristics based on the type of light that belongs to the tile
    private void UpdateLight()
    {
        if(light_type == TileLightType.normal)
        {
            hall_tile_model.GetComponent<MeshRenderer>().materials[emission_mat_id].EnableKeyword("_EMISSION");
            return;
        }

        switch (light_type)
        {
            case TileLightType.incorrect_radius:
                hall_tile_model.GetComponent<MeshRenderer>().materials[emission_mat_id].EnableKeyword("_EMISSION");
                tile_light.range = light_data.small_radius_range;
                tile_light.intensity = light_data.small_radius_intensity;
                break;
            case TileLightType.incorrect_origin:
                hall_tile_model.GetComponent<MeshRenderer>().materials[emission_mat_id].EnableKeyword("_EMISSION");
                tile_light.transform.position = new Vector3(tile_light.transform.position.x, tile_light.transform.position.y, tile_light.transform.position.z + light_data.wrong_origin_z_offset);
                break;
            case TileLightType.incorrect_temperature:
                hall_tile_model.GetComponent<MeshRenderer>().materials[emission_mat_id].EnableKeyword("_EMISSION");
                tile_light.color = new UnityEngine.Color(light_data.wrong_colour_red, light_data.wrong_colour_green, light_data.wrong_colour_blue);
                break;
            //no case for incorrect_lamp because the only characteristic it has is no emission which is the default.
            default:
                break;
        }
    }
}
