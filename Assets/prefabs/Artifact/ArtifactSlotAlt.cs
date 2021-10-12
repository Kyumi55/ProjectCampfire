using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSlotAlt : MonoBehaviour
{
    [SerializeField] Transform ArtifactSlotTrans;
    [SerializeField] GameObject ToggleObject;
    public void OnArtifactLeft()
    {
        //Debug.Log("Artifact left me");
        // platformToMove.Move(platformToMove.StartTrans);
        ToggleObject.GetComponent<Togglable>().ToggleOff();
    }

    public void OnArtifactPlaced()
    {
        //Debug.Log("Artifact placed me");
        //platformToMove.Move(platformToMove.EndTrans);
        ToggleObject.GetComponent<Togglable>().ToggleOn();
    }

    public Transform GetSlotTrans()
    {
        return ArtifactSlotTrans;
    }
}
