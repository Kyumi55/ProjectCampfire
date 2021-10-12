using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera DestinationCam;
    [SerializeField] float TransitionTime = 1.0f;
    CinemachineBrain cinemachineBrain;
    // Start is called before the first frame update
    private void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            cinemachineBrain.m_DefaultBlend.m_Time = TransitionTime;

            DestinationCam.Priority = 11;
        }
    }

    // Update is called once per frame
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            DestinationCam.Priority = 9;
        }
    }
}
