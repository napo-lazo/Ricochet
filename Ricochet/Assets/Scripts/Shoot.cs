using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private InputMaster controls;
    private bool canShoot = true;
    
    public Transform disc;
    public Transform discCheck;
    public Transform playerCamera;
    [Range(1, 90)]
    public int discAngleRange = 10;

    private void Awake()
    {
        controls = new InputMaster();
    }

    private void Start()
    {
        controls.Enable();
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(discCheck.position, Vector3.up * -1, out hit, 0.4f) && hit.collider.tag == "Disc")
        {
            Destroy(hit.collider.gameObject);
            canShoot = true;
        }

        if (controls.Player.Shoot.WasPressedThisFrame() && canShoot)
        {
            Vector3 spawnPoint = playerCamera.position;
            Quaternion discRotation = playerCamera.rotation;

            spawnPoint += transform.forward * 2f;

            float xAngle = playerCamera.rotation.eulerAngles.x;
            float zAngle = 0f;
            if (xAngle > discAngleRange + 0 && xAngle < 360 - discAngleRange)
                zAngle = 90f;

            discRotation = Quaternion.Euler(discRotation.eulerAngles.x, discRotation.eulerAngles.y, zAngle);

            Instantiate(disc, spawnPoint, discRotation);
            canShoot = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Disc")
        {
            Destroy(collision.gameObject);
            canShoot = true;
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
