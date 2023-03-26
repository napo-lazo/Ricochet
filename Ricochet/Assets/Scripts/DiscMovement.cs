using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscMovement : MonoBehaviour
{
    private InputMaster controls;
    private Rigidbody rb;
    private float timePassed = 0f;
    private bool recall = false;
    private Transform player;

    public float discSpeed = 10f;
    public float secondsFlying = 1f;
    public bool debugCollisions = false;

    public void setPlayer(Transform player)
    {
        this.player = player;
    }

    private void Awake()
    {
        controls = new InputMaster();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        controls.Enable();
        Vector3 force = transform.forward * discSpeed;
        rb.AddForce(force, ForceMode.Force);
    }

    private void Update()
    {
        if (!recall && controls.Player.CallBackDisc.WasPressedThisFrame())
            recall = true;
    }

    private void FixedUpdate()
    {
        if (!recall)
        {
            timePassed += Time.deltaTime;
            if (timePassed > secondsFlying)
            {
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.FreezeRotationY;
            }
        }
        else
        {
            Vector3 moveTo = player.position - transform.position;
            moveTo = moveTo.normalized * discSpeed;
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(moveTo.normalized, transform.up);
            rb.AddForce(moveTo, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (recall)
        {
            recall = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationY;
        }
        else
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
                transform.rotation = Quaternion.LookRotation(bounce.normalized, transform.up);
                rb.AddForce(bounce, ForceMode.Force);
            }
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
