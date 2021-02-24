using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Magnetic : MonoBehaviour
{
    NavMeshAgent nv = null;
    Vector3 direction = new Vector3();

    private void OnEnable()
    {
        if(nv == null)
        {
            nv = GetComponent<NavMeshAgent>();
        }

        if (nv == null)
        {
            nv.gameObject.AddComponent<NavMeshAgent>();
        }

        if(nv != null)
        {
            nv.enabled = true;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.mass *= 5f;
        }

        if (Random.Range(0, 2) == 1)
        {
            direction = Vector3.right;
        }
        else
        {
            direction = Vector3.left;
        }
    }

    void Update()
    {
        if(nv == null || !gameObject.IsOnScreen(Camera.main))
        {
            Destroy(gameObject);
        }
        else
        {
            nv.Move(direction * Time.deltaTime * 10f);
        }
    }
}
