using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Phone : MonoBehaviour
{
    //play audio convo if phone is near the head
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Head"))
        {
            AudioRoomManager.Instance.PlayConverstationAudio();
        }
    }

    //play the static audio if the phone is away from head
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            AudioRoomManager.Instance.PlayStaticAudio();
        }
    }
}
