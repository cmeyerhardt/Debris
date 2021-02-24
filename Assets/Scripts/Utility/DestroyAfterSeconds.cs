using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] float secondsToDestroy = 1f;


    void Update()
    {
        secondsToDestroy -= Time.deltaTime;
        if (secondsToDestroy <= 0f) 
        {
            Destroy(gameObject);
        }
    }
}
