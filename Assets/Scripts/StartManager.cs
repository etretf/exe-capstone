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
        SceneManager.LoadScene(1); // Level 1 (build index)
    }
}
