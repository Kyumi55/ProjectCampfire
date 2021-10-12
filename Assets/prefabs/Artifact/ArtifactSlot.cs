using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSlot : MonoBehaviour
{
    [SerializeField] Transform ArtifactSlotTrans;
    [SerializeField] GameObject ToggleObject;
    public void OnArtifactLeft()
    {
        Debug.Log("Artifact left me");
        ToggleObject.GetComponent<Togglable>().ToggleOff();
    }

    public void OnArtifactPlaced()
    {
        Debug.Log("Artifact placed me");
        ToggleObject.GetComponent<Togglable>().ToggleOn();
    }

    public Transform GetSlotTrans()
    {
        return ArtifactSlotTrans;
    }
}
