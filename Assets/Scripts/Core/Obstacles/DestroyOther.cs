using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOther : MonoBehaviour
{
    Obstacle target = null;
    Obstacle thisObstacle = null;

    public void SetTarget(Obstacle target, Obstacle thisObstacle)
    {
        this.target = target;
        this.thisObstacle = thisObstacle;
        Collider c = GetComponent<Collider>();
        if(c != null)
        {
            c.enabled = false;
        }
    }
    
    void Update()
    {
        if(target != null)
        {
            transform.position += (target.transform.position - transform.position) * 5f * Time.deltaTime;
            if(Vector3.Distance(target.transform.position, transform.position) < .5f)
            {
                Debug.Log("Obstacle destroying obstacle: " + name + " " + target.name);
                target.Destruct();
                thisObstacle.Destruct();
            }
        }
    }
}
