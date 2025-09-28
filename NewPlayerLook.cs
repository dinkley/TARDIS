using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerLook : MonoBehaviour
{
    private MasterInput input;
    [SerializeField] private float mouseSensitivity = 100f;

    private Vector2 look;

    private float xRotation;

    private Transform playerBody;

    private void Awake()
    {
        input = new MasterInput();
        Cursor.lockState = CursorLockMode.Locked;

        playerBody = transform.parent;
    }

    private void Update()
    {
        look = input.Player.Look.ReadValue<Vector2>();

        var mouseX = look.x * mouseSensitivity * Time.deltaTime;
        var mouseY = look.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
