using System;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    public Transform Transform;
    public float moveSpeed = 0.005f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Transform.position.z > 20 || Transform.position.z < 2)
        {
            moveSpeed = -moveSpeed;
        }

        Transform.position += new Vector3(0, 0, moveSpeed);
    }
}
