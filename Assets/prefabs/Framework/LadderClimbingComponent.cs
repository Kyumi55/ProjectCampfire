using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderClimbingComponent : MonoBehaviour
{
    
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;
    [SerializeField] float LadderHopOnTime = 0.2f;
    LadderScript CurrentClimbingLadder;
    List<LadderScript> LaddersNearby = new List<LadderScript>();
    IEnumerator SnapInfo;
    MovementComponent movementComp;
    IInputActionCollection InputAction;


    public void SetInput(IInputActionCollection inputAction)
    {
        InputAction = inputAction;
    }

    public void NotifyLadderNearby(LadderScript ladderNearby)
    {
        LaddersNearby.Add(ladderNearby);
    }

    public void NotifyLadderExit(LadderScript ladderExit)
    {
        if (ladderExit == CurrentClimbingLadder)
        {
            CurrentClimbingLadder = null;
            movementComp.SetClimbingInfo(Vector3.zero, false);
            movementComp.ClearVerticalVelocity();
        }
        LaddersNearby.Remove(ladderExit);
    }

    void HopOnLadder(LadderScript ladderToHopOn)
    {
        if (ladderToHopOn == null) return; //we don't want anything to happen if null;

        if (ladderToHopOn != CurrentClimbingLadder)
        {
            Transform snapToTransform = ladderToHopOn.GetClosestSnapTransform(transform.position);
            SnapInfo = movementComp.SnapTransform(snapToTransform); //populates variable
            StartCoroutine(SnapInfo); //starts SnapInfo(SnapTransform();
            movementComp.SetClimbingInfo(ladderToHopOn.transform.forward, true);
            //movementComp.SetIsClimbing(true);            
            CurrentClimbingLadder = ladderToHopOn;
            Invoke("EnableMovementInput", LadderHopOnTime);

        }
    }

    LadderScript FindPlayerClimbingLadder()
    {
        Vector3 PlayerDesiredMoveDir = movementComp.GetPlayerDesiredMoveDirection();
        LadderScript ChosenLadder = null;
        float ClosestAngle = 180.0f;
        foreach (LadderScript ladder in LaddersNearby)
        {
            Vector3 LadderDir = ladder.transform.position - transform.position; //Vector has magnitude and direction
            LadderDir.y = 0;
            LadderDir.Normalize();
            float Dot = Vector3.Dot(PlayerDesiredMoveDir, LadderDir);   //DOT: Using two vectors and turn it into a scaler. Scaler is cosine data
            float AngleDegrees = Mathf.Acos(Dot) * Mathf.Rad2Deg;       //acos: reverse of cosine. Rad2Deg converts radiants to degrees for cosine data
            if (AngleDegrees < LadderClimbCommitAngleDegrees && AngleDegrees < ClosestAngle)
            {
                ChosenLadder = ladder;
                ClosestAngle = AngleDegrees;
            }
        }
        return ChosenLadder;
    }

    void EnableInput()
    {
        InputAction.Enable();
    }

    void DisbleInput()
    {
        InputAction.Disable();
    }

    void Start()
    {
        movementComp = GetComponent<MovementComponent>();
    }

    void Update()
    {
        if (CurrentClimbingLadder == null)
        {
            //CurrentClimbingLadder = FindPlayerClimbingLadder();
            HopOnLadder(FindPlayerClimbingLadder());
        }/*
        if (CurrentClimbingLadder)
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

        SnapShotPositionAndRotation(); */
    }

    
}
