using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 5f;
    PlayerInputActions inputActions;
    Vector2 MoveInput;
    Vector3 Velocity;
    CharacterController characterController;

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
        //Debug.Log($"Player input is : {MoveInput}");
        Velocity = GetPlayerDesiredMoveDirection() * walkingSpeed;
        characterController.Move(Velocity * Time.deltaTime);
    }

    Vector3 GetPlayerDesiredMoveDirection()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized; //if normalized speed wont increase when pressing two buttons
    }
}
