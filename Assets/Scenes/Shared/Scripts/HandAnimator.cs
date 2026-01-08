using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

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
            var grip_val = m_GripInput.ReadValue();

            hand_animator.SetFloat("Grip", grip_val);
        } else
        {
            hand_animator.SetFloat("Grip", 0f);
        }

    }
}
