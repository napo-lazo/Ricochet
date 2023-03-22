using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private InputMaster controls;
    private bool canShoot = true;
    
    public Transform disc;
    public Transform discCheck;

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
            Vector3 spawnPoint = transform.position;
            spawnPoint += transform.forward * 2f;

            Instantiate(disc, spawnPoint, transform.rotation);
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
