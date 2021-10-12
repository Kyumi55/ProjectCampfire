using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : Pickup
{
    [SerializeField] float DropDownSlotSearchRadious = 0.2f;
    ArtifactSlotAlt CurrentSlot = null;
    //Stairs Switch = null;

    private void Start()
    {
        DroppedDown();

        /*
        SetTestNum(12f); //setter

        GetTestNum();   //getter
        */
    }

    public override void PickupBy(GameObject PickerGameObject)
    {
        base.PickupBy(PickerGameObject);
        if(CurrentSlot)
        {
            CurrentSlot.OnArtifactLeft();
            CurrentSlot = null;
        }
    }

    public override void DroppedDown()
    {
        ArtifactSlotAlt slot = GetArtifactSlotNearby();
        if(slot != null)
        {
            slot.OnArtifactPlaced();
            transform.parent = null;
            transform.rotation = slot.GetSlotTrans().rotation;
            transform.position = slot.GetSlotTrans().position;
            CurrentSlot = slot;
        }
        else
        {
            base.DroppedDown();
        }
    }

    ArtifactSlotAlt GetArtifactSlotNearby()
    {
        Collider[] Cols = Physics.OverlapSphere(transform.position, DropDownSlotSearchRadious);
        foreach(Collider col in Cols)
        {
            ArtifactSlotAlt slot = col.GetComponent<ArtifactSlotAlt>();
            if(slot != null)
            {
                return slot;
            }
        }
        return null;
    }
}
