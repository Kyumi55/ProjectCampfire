using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] Transform groundCheck;
    [SerializeField] float rotationSpeed = 5;
    PlayerInputActions inputActions;
    Vector2 MoveInput;
    Vector3 Velocity;
    float Gravity = -9.8f;
    CharacterController characterController;

    bool IsOnGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, GroundLayerMask);
    }

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>(); // give character controller component
        inputActions.Gameplay.Move1.performed += MoveInputUpdated;
        inputActions.Gameplay.Move1.canceled += MoveInputUpdated;
    }

    void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>(); //tell us the player input
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOnGround())
        {
            Velocity.y = -0.2f;
        }
        //Debug.Log($"Player input is : {MoveInput}");
        Velocity.x = GetPlayerDesiredMoveDirection().x * walkingSpeed;
        Velocity.z = GetPlayerDesiredMoveDirection().z * walkingSpeed;
        Velocity.y += Gravity * Time.deltaTime;
        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();
    }

    Vector3 GetPlayerDesiredMoveDirection()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized; //if normalized speed wont increase when pressing two buttons
    }

    void UpdateRotation()
    {
        Vector3 PlayerDisireDir = GetPlayerDesiredMoveDirection();
        if (PlayerDisireDir.magnitude == 0)
        {
            PlayerDisireDir = transform.forward;
        }
        Quaternion DesiredLocation = Quaternion.LookRotation(PlayerDisireDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredLocation, Time.deltaTime * rotationSpeed);
    }
}
