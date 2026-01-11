using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartMenu : MonoBehaviour
{
    public Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        LevelFade.Instance.LoadNextLevel();
    }

    public void StartLevel2()
    {
        LevelFade.Instance.LoadLevel(2);
    }

    public void StartLevel3()
    {
        LevelFade.Instance.LoadLevel(3);
    }

    public void StartLevel4()
    {
        LevelFade.Instance.LoadLevel(4);
    }
}
