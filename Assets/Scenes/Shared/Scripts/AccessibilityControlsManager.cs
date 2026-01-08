using UnityEngine;

public class AccessibilityControlsManager : MonoBehaviour
{
    public static AccessibilityControlsManager Instance;
    [SerializeField] GameObject tunnel_vignette_obj;
    [SerializeField] bool show_tunnel_vignette = false;
    
    private void Awake()
    {
        Instance = this;
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        toggleTunnelVignette(show_tunnel_vignette);
    }

    public void toggleTunnelVignette(bool isActive)
    {
        show_tunnel_vignette = isActive;
        tunnel_vignette_obj.SetActive(show_tunnel_vignette);
    }
}
