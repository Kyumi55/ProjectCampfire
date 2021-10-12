using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] float rotationSpeed = 5;
    [SerializeField] float EdgeCheckTracingDistance = 0.5f;
    [SerializeField] float EdgeCheckTracingDepth = 0.8f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    //[SerializeField] float groundCheckRadiuss = 1.0f;
    [SerializeField] float groundCheckRadius = 0.1f;

    bool isClimbing;
    Vector3 LadderDir;
    Vector2 MoveInput;
    Vector3 Velocity;
    float Gravity = -9.8f;
    CharacterController characterController;
    IEnumerator SnapInfo;
    [SerializeField] LayerMask GroundLayerMask;
    public float maxTime = 0.5f; //set current time

    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;
    Transform currentFloor;
    Vector3 PreviousWorldPos;
    Vector3 PreviousFloorLocalPos;
    Quaternion PreviousWorldRotation;
    Quaternion PreviousFloorLocalRotation;

    /* public void SetIsClimbing(bool climbing)
    {
        isClimbing = climbing;
    }*/

    public void SetMovementInput(Vector2 inputVal)
    {
        MoveInput = inputVal;
    }
    
    public void SetClimbingInfo(Vector3 ladderDir, bool climbing)
    {
        LadderDir = ladderDir;
        isClimbing = climbing;
    }

    public void ClearVerticalVelocity()
    {
        Velocity.y = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClimbing)
        {
            CalculateClimbingVelocity();
        }
        else
        {
            CalculateWalkingVelocity();
        }

        CheckFloor();
        FollowFloor();

        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();

        SnapShotPositionAndRotation();
    }

    
    void FollowFloor()
    {
        if (currentFloor)
        {
            Vector3 DeltaMove = currentFloor.TransformPoint(PreviousFloorLocalPos) - PreviousWorldPos;
            Velocity += DeltaMove / Time.deltaTime;

            Quaternion DestinationRot = currentFloor.rotation * PreviousFloorLocalRotation;
            Quaternion DeltaRot = Quaternion.Inverse(PreviousWorldRotation) * DestinationRot;
            transform.rotation = transform.rotation * DeltaRot;
        }
    }

    void CalculateClimbingVelocity()
    {
        if (MoveInput.magnitude == 0)
        {
            return;
        }
                
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDirection();

        float Dot = Vector3.Dot(LadderDir, PlayerDesiredDir);

        if (Dot < 0)
        {
            Velocity = GetPlayerDesiredMoveDirection() * walkingSpeed;
            Velocity.y = walkingSpeed;
        }
        else
        {
            if (IsOnGround())
            {
                Velocity = GetPlayerDesiredMoveDirection() * walkingSpeed;
            }
            Velocity.y = -walkingSpeed;
        }
    }

    void SnapShotPositionAndRotation()
    {
        PreviousWorldPos = transform.position;
        PreviousWorldRotation = transform.rotation;
        if (currentFloor != null)
        {
            PreviousFloorLocalPos = currentFloor.InverseTransformPoint(transform.position);
            PreviousFloorLocalRotation = Quaternion.Inverse(currentFloor.rotation) * transform.rotation;
            //to add to rotation, you do QuaternionA * QuaternionB
            //to subtract, you do Quaternion.Inverse(QuaternionA) * QuaternionB

            //LocRot + FloorRot = WorldRot
            //LocRot = WorldRot - FloorRot
        }

    }

    public Vector3 GetPlayerDesiredMoveDirection()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized; //if normalized speed wont increase when pressing two buttons
    }

    void UpdateRotation()
    {
        if (isClimbing)
        {
            return;
        }
        Vector3 PlayerDisireDir = GetPlayerDesiredMoveDirection();
        if (PlayerDisireDir.magnitude == 0)
        {
            PlayerDisireDir = transform.forward;
        }
        Quaternion DesiredLocation = Quaternion.LookRotation(PlayerDisireDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredLocation, Time.deltaTime * rotationSpeed);
        //Lerping eases in and out 
        //SYNTAX: Quaternion.Lerp(Origin/Start, Destination, given time period);
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

    void CheckFloor()
    {
        Collider[] cols = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, GroundLayerMask);
        if (cols.Length != 0)
        {
            if (currentFloor != cols[0].transform)
            {
                currentFloor = cols[0].transform;
                SnapShotPositionAndRotation();
            }
        }
    }

    bool IsOnGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, GroundLayerMask);
    }

    //Changed Coding for ladder to fix bug
    public IEnumerator SnapTransform(Transform SnapToTransformVar)
    {
        float currentTime = 0f; //set current time


        //Quaternion: Rotation in three dimensional space
        Quaternion StartRotation = transform.rotation;
        Vector3 StartPostiion = transform.position;


        //mini update
        while (currentTime < maxTime)
        {
            currentTime += Time.deltaTime; //

            //characterController.Move(SnapToTransformVar.position - transform.position); //<
            //SnapToTransformVar.rotation = SnapToTransformVar.rotation; //<

            transform.rotation = Quaternion.Lerp(StartRotation, SnapToTransformVar.rotation, currentTime / maxTime);
            Vector3 UpdatePos = Vector3.Lerp(StartPostiion, SnapToTransformVar.position, currentTime / maxTime) - transform.position;

            characterController.Move(UpdatePos);

            yield return new WaitForEndOfFrame();

        }

    }
    
}
