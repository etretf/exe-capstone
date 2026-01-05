using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public InputActionProperty pauseButtonAction;

    private bool isPaused = false;

    void Update()
    {
        // One button to rule them all
        if (pauseButtonAction.action.WasPressedThisFrame())
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f; // Stops physics and time-based movement
        isPaused = true;
        PositionMenuInFront();
    }

    private void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f; // Returns game to normal speed
        isPaused = false;
    }

    void PositionMenuInFront()
    {
        Transform cameraTransform = Camera.main.transform;
        // Rotate it to face the headset perfectly
        pauseMenuCanvas.transform.LookAt(cameraTransform.position);
        pauseMenuCanvas.transform.Rotate(0, 180, 0); // Corrects the UI being backwards
    }
}