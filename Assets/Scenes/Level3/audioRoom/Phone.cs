using UnityEngine;

public class Phone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Head"))
        {
            AudioRoomManager.Instance.PlayConverstationAudio();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            AudioRoomManager.Instance.PlayStaticAudio();
        }
    }
}
