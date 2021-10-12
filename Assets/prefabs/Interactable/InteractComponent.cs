using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    List<Interactable> interactable = new List<Interactable>();
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
        Interactable otherAsInteractable = other.GetComponent<Interactable>();
        if(otherAsInteractable != null)
        {
            //Debug.Log("HI");
            if (!interactable.Contains(otherAsInteractable)) //checks to see if it is already added or not.
            {
                interactable.Add(otherAsInteractable); //adds it
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable otherAsInteractable = other.GetComponent<Interactable>();
        if (otherAsInteractable != null)
        {
            if (!interactable.Contains(otherAsInteractable)) //checks to see if it is already added or not.
            {
                interactable.Remove(otherAsInteractable); //adds it
            }
        }
    }

    public void Interact()
    {
        Interactable clossestInteractable = GetClosestInteractable();
        if(clossestInteractable != null)
        {
            clossestInteractable.Interact(transform.parent.gameObject);
        }
    }

    Interactable GetClosestInteractable()
    {
        Interactable closestInteractable = null;
        
        if(interactable.Count == 0)
        {
            return closestInteractable;
        }

        float ClosestDist = float.MaxValue;

        foreach(var ItemInteractable in interactable)
        {
            float Dist = Vector3.Distance(transform.position, ItemInteractable.transform.position);

            if(Dist < ClosestDist)
            {
                closestInteractable = ItemInteractable;
                ClosestDist = Dist;
            }
        }

        return closestInteractable;
    }
}
