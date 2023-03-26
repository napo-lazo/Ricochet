using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float timePassed = 0f;

    public float discSpeed = 10f;
    public float secondsFlying = 1f;
    public bool debugCollisions = false;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Vector3 force = transform.forward * discSpeed;
        rb.AddForce(force, ForceMode.Force);
    }

    private void FixedUpdate()
    {
        timePassed += Time.deltaTime;
        if (timePassed > secondsFlying)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationY;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!rb.useGravity)
        {
            timePassed = 0f;
            ContactPoint contact = collision.GetContact(0);
            Vector3 bounce = Vector3.Reflect(-collision.relativeVelocity, contact.normal);
            bounce = bounce.normalized * discSpeed;

            if (debugCollisions)
            {
                Debug.DrawLine(contact.point, contact.point + contact.normal, Color.red, 5f, false);
                Debug.DrawLine(contact.point, contact.point + collision.relativeVelocity, Color.blue, 5f, false);
                Debug.DrawLine(contact.point, contact.point + Vector3.Reflect(-collision.relativeVelocity, contact.normal), Color.green, 5f, false);
            }

            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(bounce.normalized, transform.transform.up);
            rb.AddForce(bounce, ForceMode.Force);
        }
    }
}
