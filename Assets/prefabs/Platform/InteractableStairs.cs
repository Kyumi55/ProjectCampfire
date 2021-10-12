using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStairs : MonoBehaviour
{
    [SerializeField] Platform StairsToMove;
    //[SerializeField] float floatNum = 5f;


    private void OnTriggerEnter(Collider other)
    {
        InteractComponent interactableComp = other.GetComponent<InteractComponent>();
        if (interactableComp != null)
        {
            StairsActivate1();
        }
    }

    public void StairsActivate1()
    {
        StairsToMove.Move(StairsToMove.StartTrans);
    }

    public void StairsActivate2()
    {
        StairsToMove.Move(StairsToMove.EndTrans);
    }
}
