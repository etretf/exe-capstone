using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

// Referenced https://www.youtube.com/watch?v=qQqNQ4y-cU8 from Fist Full of Shrimp and the existing ControllerAnimator script

public class HandAnimator : MonoBehaviour
{
    [SerializeField] GameObject hand_prefab;
    Animator hand_animator;
    [SerializeField] XRInputValueReader<float> m_GripInput = new XRInputValueReader<float>("Grip");
    [SerializeField] XRInputValueReader<float> m_TriggerInput = new XRInputValueReader<float>("Trigger");

    void Start()
    {
        GameObject spawned_hand = Instantiate(hand_prefab, transform, false);
        hand_animator = spawned_hand.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (m_GripInput != null)
        {
            float grip_val = m_GripInput.ReadValue();

            hand_animator.SetFloat("Grip", grip_val);
        } else
        {
            hand_animator.SetFloat("Grip", 0f);
        }

        if (m_TriggerInput != null)
        {
            float trigger_val = m_TriggerInput.ReadValue();

            hand_animator.SetFloat("Trigger", trigger_val);
        } else
        {
            hand_animator.SetFloat("Trigger", 0f);
        }

    }
}
