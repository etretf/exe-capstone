using System.Collections;
using UnityEngine;

public class LevelDoorActions : MonoBehaviour
{
    public virtual void OpenDoor()
    {
        gameObject.GetComponent<Door>().OpenDoor();
        GoToNextLevel();
    }

    public void GoToNextLevel()
    {
        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(AllConstants.DOOR_DELAY * 1.5f);
        LevelFade.Instance.LoadNextLevel();
    }
}
