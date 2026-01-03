using System.Collections;
using UnityEngine;


public class HallwayTile : MonoBehaviour
{
    public int index;
    [SerializeField] Light tile_light;
    private Door door;
    public AudioSource light_audio_src;
    public AudioClip door_delay_sfx;
    public AudioClip door_open_sfx;
    public AudioClip door_close_sfx;
    public AudioClip light_hum_start_sfx;
    public AudioClip light_hum_sfx;

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

    IEnumerator playLightSound()
    {
        light_audio_src.clip = light_hum_start_sfx;
        light_audio_src.Play();

        if (tile_light != null)
        {
            tile_light.enabled = true;
        }

        yield return new WaitForSeconds(light_hum_start_sfx.length);

        light_audio_src.clip = light_hum_sfx;
        light_audio_src.loop = true;
        light_audio_src.Play();
    }


    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (tile_light != null)
        {
            tile_light.enabled = false;
        }

        if (light_audio_src != null)
        {
            light_audio_src.Stop();
        }
        
    }
}
