using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offsetVector;
    public float speed;

    private Rigidbody rb;

    private void Start()
    {
        rb = player.GetComponent<Rigidbody>();

    }
    private void FixedUpdate()
    {
        Vector3 playerForwardVector = (rb.velocity + player.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position,
            player.position + player.transform.TransformVector(offsetVector)
            + playerForwardVector * (-5f), speed * Time.deltaTime);
        transform.LookAt(player);
    }
}
