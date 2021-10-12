using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamTransComponent : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera DestinationCam;
    [SerializeField] float TransitionTime = 1.0f;
    public CinemachineBrain cinemachineBrain;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //Set everything first
    public void SetTransTime(float transTime)
    {
        transTime = TransitionTime;
    }

    public void SetCineMachineBrain(CinemachineBrain cineBrainSet)
    {
        cineBrainSet = cinemachineBrain;
    }

    public void SetDestinationCam(CinemachineVirtualCamera destCamSet)
    {
        destCamSet = DestinationCam;
    }

    //call this to change the DestinationCam's priority
    public void DestCamPriority()
    {
        DestinationCam.Priority = 9;
    }

    public void GrabColComp(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            cinemachineBrain.m_DefaultBlend.m_Time = TransitionTime;

            DestinationCam.Priority = 11;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
