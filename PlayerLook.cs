using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private Transform playerBody;

    private float xAxisClamp;

    private MasterInput controls;

    private Vector2 look;

    private void Awake()
    {
        LockCursor();
        controls = new MasterInput();
        xAxisClamp = 0.0f;
    }


    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        look = controls.Player.Look.ReadValue<Vector2>();
        //Keeping these here incase it breaks, which it probably will.
        //Anyway, updating input system to the new Unity Input System
        //float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        float mouseX = look.x * mouseSensitivity * Time.deltaTime;
        float mouseY = look.y * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if(xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
        
    }

    //TODO: Replace this DOGSHIT with a single Mathf.Clamp call
    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}