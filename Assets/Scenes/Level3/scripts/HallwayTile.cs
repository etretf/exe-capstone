using System.Collections;
using System.Drawing;
using UnityEngine;


public class HallwayTile : MonoBehaviour
{
    public int index;
    [SerializeField] Light tile_light;
    [SerializeField] GameObject hall_tile_model;
    [SerializeField] int emission_mat_id;

    private Door door;
    public AudioSource light_audio_src;
    public AudioClip door_delay_sfx;
    public AudioClip door_open_sfx;
    public AudioClip door_close_sfx;
    public AudioClip light_hum_start_sfx;
    public AudioClip[] light_hum_clips_sfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        door = gameObject.GetComponentInChildren<Door>();   
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

        if(hall_tile_model!=null)
            hall_tile_model.GetComponent<MeshRenderer>().materials[emission_mat_id].EnableKeyword("_EMISSION");
        //hall_tile_model.GetComponent<EmissionControl>().EnableEmmission();

        yield return new WaitForSeconds(light_hum_start_sfx.length);

        light_audio_src.clip = light_hum_clips_sfx[Random.Range(0, light_hum_clips_sfx.Length)];
        light_audio_src.loop = true;
        light_audio_src.Play();
    }
}
