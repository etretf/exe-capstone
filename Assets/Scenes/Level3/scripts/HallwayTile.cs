using UnityEngine;


public class HallwayTile : MonoBehaviour
{
    public int index;
    [SerializeField] Light tile_light;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (tile_light != null)
        {
            tile_light.enabled = true;
        }
        TileGenerationManager.Instance.UpdateTilesToRender(index);
        
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (tile_light != null)
        {
            tile_light.enabled = false;
        }
    }
}
