using UnityEngine;

public class EmissionControl : MonoBehaviour
{
    [SerializeField] private Material door_emmissive_material;

    private void Start()
    {
        //if (door_emmissive_material != null)
        //    door_emmissive_material = new Material(door_emmissive_material);
    }

    public void EnableEmmission()
    {
        if (door_emmissive_material != null)
        {
            door_emmissive_material.EnableKeyword("_EMISSION");
        }
    }

    public void DisableEmmission()
    {
        if (door_emmissive_material)
        {
            door_emmissive_material.DisableKeyword("_EMISSION");
        }
    }
}
