using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;

    [SerializeField] private float walkSpeed, runSpeed;
    [SerializeField] private float runBuildUpSpeed;
    [SerializeField] private KeyCode runKey;

    private float movementSpeed;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;
    private DebugController debugController;
    
    private CharacterController charController;
    
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    private MasterInput input;
    private Vector2 move;

    private bool isJumping;

    private void Awake()
    {
        input = new MasterInput();
        charController = GetComponent<CharacterController>();
        debugController = GetComponent<DebugController>();
    }

    private void Update()
    {
        //Temporary fix for player movement. Integrate into PlayerMovement() soon, because this 'fix' disables gravity. oops..
        if(!debugController.isMapOpen)
        {
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
        //float horizInput = Input.GetAxis("Horizontal");
        //float vertInput = Input.GetAxis("Vertical");
        move = input.Player.Move.ReadValue<UnityEngine.Vector2>();
        float horizInput = move.x;
        float vertInput = move.y;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;


        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        if ((vertInput != 0 || horizInput != 0) && OnSlope())
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);


        SetMovementSpeed();
        JumpInput();
    }

    private void SetMovementSpeed()
    {
        if (input.Player.Sprint.triggered)
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        else
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
    }


    private bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
            {
                return true;
            }
                
        return false;
    }

    private void JumpInput()
    {
        //if(Input.GetButtonDown("Jump") && !isJumping)
        if(input.Player.Jump.triggered && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }


    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
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