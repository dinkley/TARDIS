using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMove : MonoBehaviour
{
    private MasterInput input;
    [SerializeField] private float movementSpeed = 10;

    private Vector2 move;
    private GameObject camera;

    private CharacterController characterController;

    private void Awake()
    {
        input = new MasterInput();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        move = input.Player.Move.ReadValue<Vector2>();

        Vector3 movement = (move.y * transform.forward) + (move.x * transform.right) * Time.deltaTime;

        characterController.Move(movement * movementSpeed * Time.deltaTime);
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
