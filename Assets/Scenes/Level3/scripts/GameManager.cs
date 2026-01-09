using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool is_player_in_hallway = true;

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

    public bool GetIsPlayerInHallway()
    {
        return is_player_in_hallway;
    }

    public void SetPlayerNotInHallway()
    {
        is_player_in_hallway = false;
    }

    public void SetPlayerInHallway()
    {
        is_player_in_hallway = true;
    }
}
