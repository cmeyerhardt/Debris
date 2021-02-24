using System;
using UnityEngine;
using UnityEngine.Events;

public class ClickForPoints : MonoBehaviour
{
    public bool pointsEarned = false;
    [SerializeField] public int numPoints = 1;
    public FallingObjectType type = FallingObjectType.A;

    public IntEvent intEvent = new IntEvent();
    public BoolEvent boolEvent = new BoolEvent();
    public UnityEvent impact = null;

    public bool destroy = false;
    //public bool canSpawn2D = false;
    internal bool frozen = false;

    [SerializeField] internal GameObject successFX = null;
    [SerializeField] internal GameObject failFX = null;
    
    public Transform displayRoot = null;
    public AudioMod audioMod = null;

    public virtual void SuccessfullyClick()
    {
        if(pointsEarned) { return; }
        pointsEarned = true;

        if (successFX != null)
            Instantiate(successFX, transform.position, Quaternion.identity, null);
        intEvent.Invoke(numPoints);
        boolEvent.Invoke(true);
    }



    public virtual void ImpactFloor()
    {
        if (!gameObject.IsOnScreen(Camera.main)) { Debug.Log("Not on screen"); Destroy(gameObject); return; }

        if (!pointsEarned)
        {
            if (failFX != null)
                Instantiate(failFX, transform.position, Quaternion.identity, null);
            boolEvent.Invoke(false);
            Destroy(gameObject);
        }
        if(impact != null)
        {
            impact.Invoke();
        }
    }
     
    public virtual void Freeze(float duration = 5f)
    {
        frozen = true;
        Invoke("UnFreeze", duration);
    }

    public virtual void UnFreeze()
    {
        frozen = false;
    }

    public FallingObjectType GetObjectType()
    {
        return type;
    }

    public void DestroyIfMatchType(FallingObjectType typeToMatch)
    {
        if(typeToMatch == type && !pointsEarned)
        {
            SuccessfullyClick();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
