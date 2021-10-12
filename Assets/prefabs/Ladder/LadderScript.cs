using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{
    [SerializeField] Transform TopSnapTransform;
    [SerializeField] Transform BottomSnapTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        LadderClimbingComponent otherClimbingComp = other.GetComponent<LadderClimbingComponent>();
        if(otherClimbingComp != null)
        {
            otherClimbingComp.NotifyLadderNearby(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LadderClimbingComponent otherClimbingComp = other.GetComponent<LadderClimbingComponent>();
        if (otherClimbingComp != null)
        {
            otherClimbingComp.NotifyLadderExit(this);
        }
    }

    public Transform GetClosestSnapTransform(Vector3 position)
    {
        float DistanceToTop = Vector3.Distance(position, TopSnapTransform.position);
        float DistanceToBot = Vector3.Distance(position, BottomSnapTransform.position);
        return DistanceToTop < DistanceToBot ? TopSnapTransform : BottomSnapTransform;
        //^if distanceToTop is < DistanceToBot, return TopSnapTransform. Else return BottomSnapToTransform
    }
}
