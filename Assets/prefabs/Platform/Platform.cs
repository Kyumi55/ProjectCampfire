using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, Togglable
{
    [SerializeField] Transform objectMove;
    [SerializeField] float transitionTime;

    public Transform StartTrans;
    public Transform EndTrans;


    Coroutine MovingCoroutine;

    public void ToggleOn()
    {
        Move(true);
    }

    public void ToggleOff()
    {
        Move(false);
    }

    public void Move(bool ToEnd)
    {
        if (ToEnd)
        {
            Move(EndTrans);
        }
        else
        {
            Move(StartTrans);
        }
    }

    public void Move(Transform Destination)
    {
        if(MovingCoroutine != null)
        {
            StopCoroutine(MovingCoroutine);
            MovingCoroutine = null;
        }
        MovingCoroutine = StartCoroutine(MoveToTrans(Destination, transitionTime));
    }

    public IEnumerator MoveToTrans(Transform Destination, float TransitionTime)
    {
        float timer = 0f;
        while(timer < TransitionTime)
        {
            timer += Time.deltaTime;
            objectMove.position = Vector3.Lerp(objectMove.position, Destination.position, timer / TransitionTime);
            objectMove.rotation = Quaternion.Lerp(objectMove.rotation, Destination.rotation, timer / TransitionTime);

            yield return new WaitForEndOfFrame();
        }
    }
}
