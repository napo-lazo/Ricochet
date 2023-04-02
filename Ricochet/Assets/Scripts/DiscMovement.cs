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
    private List<int> idsOfLastEnemiesHit = new List<int>();

    [Header("Movement settings")]
    public float discSpeed = 10f;
    public float secondsFlying = 1f;

    [Header("Bounce settings")]
    [Tooltip("How many previously hit enemies to keep track of to prevent the disc from getting stuck on a bounce loop")]
    public int EnemyBounceLoopPrevention = 2;

    [Header("Field of view")]
    public float viewRadius = 25f;
    [Range(0, 360)]
    public float viewAngle = 70f;
    
    [Header("Debug settings")]
    public bool debugCollisions = false;

    [Header("Layers")]
    public LayerMask enemyMask;
    public LayerMask obstacleMask;

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
            if (player != null)
            {
                Vector3 moveTo = player.position - transform.position;
                moveTo = moveTo.normalized * discSpeed;
                rb.velocity = Vector3.zero;
                transform.rotation = Quaternion.LookRotation(moveTo.normalized, transform.up);
                rb.AddForce(moveTo, ForceMode.Force);
            }
        }
    }

    private Collider GetNearestEnemy()
    {
        float nearestDistance = 0;
        Collider nearestEnemy = null;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);

        foreach (Collider collider in targetsInViewRadius)
        {
            Transform target = collider.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                int instanceId = collider.GetInstanceID();
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!idsOfLastEnemiesHit.Contains(instanceId) && !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    if (distanceToTarget < nearestDistance || nearestDistance == 0)
                    {
                        nearestDistance = distanceToTarget;
                        nearestEnemy = collider;
                    }
                }
            }
        }

        return nearestEnemy;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enemyMask ==  (enemyMask | (1 << collision.gameObject.layer)))
        {
            idsOfLastEnemiesHit.Add(collision.collider.GetInstanceID());
            if (idsOfLastEnemiesHit.Count > EnemyBounceLoopPrevention)
                idsOfLastEnemiesHit.RemoveAt(0);
        }

        if (player != null && recall)
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
                    Debug.DrawLine(contact.point, contact.point + contact.normal, Color.red);
                    Debug.DrawLine(contact.point, contact.point + collision.relativeVelocity, Color.blue);
                    Debug.DrawLine(contact.point, contact.point + Vector3.Reflect(-collision.relativeVelocity, contact.normal), Color.green);
                    Debug.Break();
                }

                rb.velocity = Vector3.zero;
                transform.rotation = Quaternion.LookRotation(bounce.normalized, transform.up);

                //bounce assist
                Collider nearestEnemy = GetNearestEnemy();
                if (nearestEnemy != null)
                {
                    if (debugCollisions)
                    {
                        Debug.DrawRay(transform.position, nearestEnemy.transform.position - transform.position, Color.magenta);
                        Debug.Break();
                    }

                    bounce = (nearestEnemy.transform.position - transform.position).normalized * discSpeed;
                    transform.rotation = Quaternion.LookRotation(bounce.normalized, transform.up);
                }

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
