using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;
    Vector2 MoveInput;
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
        Debug.Log($"Player input is : {MoveInput}");
    }
}
