using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactable
{

    //[SerializeField] float TESTnum = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
     //function for setting
    public void SetTestNum(float NEWnum)
    {
        TESTnum = NEWnum;
    }

     //function for getting
    public float GetTestNum()
    {
        return TESTnum;
    }
    */
    public virtual void PickupBy(GameObject PickerGameObject)
    {
        Transform PickUpSocketTransform = PickerGameObject.transform;

        Player PickerPlayer = PickerGameObject.GetComponent<Player>();

        if(PickerPlayer!= null)
        {
            PickUpSocketTransform = PickerPlayer.GetPickedUpSocketTransform();
        }

        transform.rotation = PickUpSocketTransform.transform.rotation;
        transform.parent = PickUpSocketTransform.transform;
        transform.localPosition = Vector3.zero;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public virtual void DroppedDown()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;

    }


    public override void Interact(GameObject InteractingObject)
    {
        if (transform.parent != null)
        {
            DroppedDown();
        }
        else
        {
            //Debug.Log($"I am interacting with: {InteractingObject}");
            Vector3 DirFromInteractingGameObj = (transform.position - InteractingObject.transform.position).normalized;
            Vector3 DirOfInteractingGameObj = InteractingObject.transform.forward; //forward direction of the player
            float Dot = Vector3.Dot(DirOfInteractingGameObj, DirFromInteractingGameObj);
            if(Dot > 0.5f)
            {
                PickupBy(InteractingObject);
            }
        }
    }
}
