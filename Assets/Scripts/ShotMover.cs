using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShotMover : NetworkBehaviour
{
    public float speed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }
}

