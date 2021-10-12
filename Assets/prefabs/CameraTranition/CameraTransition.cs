using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    //[SerializeField] CinemachineVirtualCamera DestinationCam;
    //[SerializeField] float TransitionTime = 1.0f;
    //CinemachineBrain cinemachineBrain;
    // Start is called before the first frame update

    public CamTransComponent otherCamTrans;

    private void Start()
    {
        //call the cinemachineBrain and properly set it
        otherCamTrans.cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            //Call the other script CamTransComponent
            otherCamTrans.GrabColComp(other);
        }
    }

    // Update is called once per frame
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            //Call the other script CamTransComponent
            otherCamTrans.DestCamPriority();
        }
    }
}
