using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaClick : MonoBehaviour
{
    [SerializeField] public float radius = 3f;
    [SerializeField] Transform scale = null;

    private void Awake()
    {
        Obstacle o = GetComponentInParent<Obstacle>();
        if(o != null)
        {
            o.SetAreaClick(this);
        }

        if(GetComponent<Rigidbody>() == null)
        {
            Rigidbody r = gameObject.AddComponent<Rigidbody>();
            r.isKinematic = true;
            r.useGravity = false;
        }
        SetRadius(3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void ClickInArea()
    {
        ClickForPoints[] array = FindObjectsOfType<ClickForPoints>();
        List<ClickForPoints> list = new List<ClickForPoints>();
        foreach (ClickForPoints clickable in array)
        {
            if(clickable != null)
            {
                if (Vector3.Distance(clickable.transform.position, transform.position) <= radius)
                {
                    list.Add(clickable);
                }
            }
        }

        foreach (ClickForPoints clickable in list)
        {
            if (clickable != null && clickable.enabled && clickable.gameObject.activeInHierarchy)
                clickable.SuccessfullyClick();
        }
    }

    public void ModifyRadius(float r)
    {
        SetRadius(radius * r);
    }

    public void SetRadius(float r)
    {
        radius = r;
        if(scale != null)
        {
            scale.localScale = radius.ToVector3();
        }
    }
}
