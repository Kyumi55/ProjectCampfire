using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] float EdgeCheckTracingDistance = 0.5f;
    [SerializeField] float EdgeCheckTracingDepth = 0.8f;
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] LayerMask LadderLayerMask;
    [SerializeField] Transform groundCheck;

    LadderScript CurrentClimbingLadder;
    List<LadderScript> LaddersNearby = new List<LadderScript>();


    [SerializeField] float rotationSpeed = 5;
    PlayerInputActions inputActions;
    Vector2 MoveInput;
    Vector3 Velocity;
    float Gravity = -9.8f;
    CharacterController characterController;

    public void NotifyLadderNearby(LadderScript ladderNearby)
    {
        LaddersNearby.Add(ladderNearby);
    }

    public void NotifyLadderExit(LadderScript ladderExit)
    {
        if(ladderExit == CurrentClimbingLadder)
        {
            CurrentClimbingLadder = null;
            Velocity.y = 0;
        }
        LaddersNearby.Remove(ladderExit);
    }

    

    LadderScript FindPlayerClimbingLadder()
    {
        Vector3 PlayeresiredMoveDir = GetPlayerDesiredMoveDirection();
        LadderScript ChosenLadder = null;
        float ClosestAngle = 180.0f;
        foreach(LadderScript ladder in LaddersNearby)
        {
            Vector3 LadderDir = ladder.transform.position - transform.position;
            LadderDir.y = 0;
            LadderDir.Normalize();
            float Dot = Vector3.Dot(PlayeresiredMoveDir, LadderDir);
            float AngleDegrees = Mathf.Acos(Dot) * Mathf.Rad2Deg;
            if(AngleDegrees < LadderClimbCommitAngleDegrees && AngleDegrees < ClosestAngle)
            {
                ChosenLadder = ladder;
                ClosestAngle = AngleDegrees;
            }
        }
        return ChosenLadder;
    }

    bool IsOnGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, GroundLayerMask);
    }

    bool IsTouchingLadder()
    {
        Vector3 downward = transform.TransformDirection(Vector3.down) * 10;
        Debug.DrawRay(transform.position, downward, Color.green, 500f);
        print("WAO");
        Ray ray = new Ray(transform.position, transform.up * -1);
        RaycastHit hit;
        float distance = 18f;
        Physics.Raycast(ray, out hit, distance);
        if (hit.distance > 0.5f)
        {
            print("Touching Ladder!");
            return (true);
        }
        else
        {
            print("Not yet");
            return (false);
        }
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

    void HopOnLadder(LadderScript ladderToHopOn)
    {
        if (ladderToHopOn == null) return; //we don't want anything to happen if null;

        if(ladderToHopOn != CurrentClimbingLadder)
        {
            Transform snapToTransform = ladderToHopOn.GetClosestSnapTransform(transform.position);
            characterController.Move(snapToTransform.position - transform.position);
            transform.rotation = snapToTransform.rotation;
            CurrentClimbingLadder = ladderToHopOn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentClimbingLadder == null)
        {
            //CurrentClimbingLadder = FindPlayerClimbingLadder();
            HopOnLadder(FindPlayerClimbingLadder());
        }
        if (CurrentClimbingLadder)
        {
            CalculateClimbingVelocity();
        }
        else
        {
            CalculateWalkingVelocity();
        }


        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();
    }

    void CalculateClimbingVelocity()
    {
        if (MoveInput.magnitude == 0)
        {
            return;
        }

        Vector3 LadderDir = CurrentClimbingLadder.transform.forward;
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDirection();

        float Dot = Vector3.Dot(LadderDir, PlayerDesiredDir);

        if(Dot < 0)
        {
            Velocity = GetPlayerDesiredMoveDirection() * walkingSpeed;
            Velocity.y = walkingSpeed;
        }
        else
        {
            if(IsOnGround())
            {
                Velocity = GetPlayerDesiredMoveDirection() * walkingSpeed;
            }
            Velocity.y = -walkingSpeed;
        }
    }

    private void CalculateWalkingVelocity()
    {
        if (IsOnGround())
        {
            Velocity.y = -0.2f;
        }

        //Debug.Log($"Player input is : {MoveInput}");
        Velocity.x = GetPlayerDesiredMoveDirection().x * walkingSpeed;
        Velocity.z = GetPlayerDesiredMoveDirection().z * walkingSpeed;

        Velocity.y += Gravity * Time.deltaTime;

        Vector3 PosXTracePos = transform.position + new Vector3(EdgeCheckTracingDistance, 0.5f, 0f);
        Vector3 NegXTracePos = transform.position + new Vector3(-EdgeCheckTracingDistance, 0.5f, 0f);
        Vector3 NegZTracePos = transform.position + new Vector3(0f, 0.5f, -EdgeCheckTracingDistance);
        Vector3 PosZTracePos = transform.position + new Vector3(0f, 0.5f, EdgeCheckTracingDistance);

        bool CanGoPosX = Physics.Raycast(PosXTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoNegX = Physics.Raycast(NegXTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoPosZ = Physics.Raycast(PosZTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoNegZ = Physics.Raycast(NegZTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);

        float xMin = CanGoNegX ? float.MinValue : 0f;
        float xMax = CanGoPosX ? float.MaxValue : 0f;
        float zMin = CanGoNegZ ? float.MinValue : 0f;
        float zMax = CanGoPosZ ? float.MaxValue : 0f;

        Velocity.x = Mathf.Clamp(Velocity.x, xMin, xMax);
        Velocity.z = Mathf.Clamp(Velocity.z, zMin, zMax);
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
        if(CurrentClimbingLadder!= null)
        {
            return;
        }
        Quaternion DesiredLocation = Quaternion.LookRotation(PlayerDisireDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredLocation, Time.deltaTime * rotationSpeed);
    }

    
}
