using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;
    
    [SerializeField] LayerMask LadderLayerMask;
    [SerializeField] Transform PickupSocketTransform;

    LadderClimbingComponent climbingComp;

    //LadderScript CurrentClimbingLadder;
    //List<LadderScript> LaddersNearby = new List<LadderScript>();
    public float LadderHopOnTime = 0.5f; //set current time
    PlayerInputActions inputActions;
    //Vector2 MoveInput;
    MovementComponent movementComp;
    //CharacterController characterController;

   
    public Transform GetPickedUpSocketTransform()
    {
        return PickupSocketTransform;
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
        movementComp = GetComponent<MovementComponent>(); // give character controller component
        climbingComp = GetComponent<LadderClimbingComponent>();
        climbingComp.SetInput(inputActions);
        inputActions.Gameplay.Move1.performed += MoveInputUpdated;
        inputActions.Gameplay.Move1.canceled += MoveInputUpdated;
        inputActions.Gameplay.Interact.performed += Interact;
    }

    void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        movementComp.SetMovementInput(ctx.ReadValue<Vector2>()); //tell us the player input
        //MovementComponent.SetMovementInput(ctx.ReadValue<Vector2>());
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        InteractComponent interactComp = GetComponentInChildren<InteractComponent>();
        if(interactComp!= null)
        {
            interactComp.Interact();
        }
    }

    

}
