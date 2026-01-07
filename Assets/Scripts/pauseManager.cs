using UnityEngine;
using UnityEngine.InputSystem;

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
        // Positions the menu 2 meters in front of the VR headset
        Transform cameraTransform = Camera.main.transform;
        pauseMenuCanvas.transform.position = cameraTransform.position + cameraTransform.forward * 2.0f;

        // Makes the menu face the player
        pauseMenuCanvas.transform.LookAt(new Vector3(cameraTransform.position.x, pauseMenuCanvas.transform.position.y, cameraTransform.position.z));
        pauseMenuCanvas.transform.forward *= -1; // Corrects UI mirroring
    }
}