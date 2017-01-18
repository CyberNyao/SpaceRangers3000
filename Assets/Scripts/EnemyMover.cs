using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyMover : NetworkBehaviour
{
    public float amplitude;
    public float frequency;
    [SyncVar, HideInInspector]
    public float speed;
    
    [SyncVar]
    private double spawnTime;
    [SyncVar]
    private Vector3 orthogonal;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (isServer)
        {
            spawnTime = Network.time;
            var direction = new Vector3(transform.position.x, 0, -transform.position.z).normalized;
            orthogonal = new Vector3(-direction.z, 0, direction.x);
        }
    }

    void FixedUpdate()
    {
        float t = (float)(Network.time - spawnTime);
        rb.velocity = new Vector3(0, 0, 1) * -speed + orthogonal * amplitude *  Mathf.Sin(frequency*t);
    }
}
