using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private InputMaster controls;
    private float xRotation = 0f;

    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private void Awake()
    {
        controls = new InputMaster();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controls.Enable();
    }

    void Update()
    {
        float mouseX = controls.Player.Aim.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
        float mouseY = controls.Player.Aim.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
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
