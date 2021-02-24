using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour { }
public class ObstacleSpawner : Spawner
{
    //todo - use a grid to position obstacles
    //spawn pattern can vary level difficulty
    //example 1: all cubes spawn in first row. PYP: cube row moves up
    //example 2: cubes spawn in columns until top, then next column
        
    Dictionary<Obstacle, Vector2> obstacleDict = new Dictionary<Obstacle, Vector2>();

    internal override void Start()
    {
        base.Start();
    }

    public override void VarySize(GameObject o)
    {
        o.transform.localScale = new Vector3(o.transform.localScale.x, o.transform.localScale.y, spawnBoxSize.z);
    }

    public override Vector3 GetPosition()
    {
        Vector3 original = new Vector3();
        bool done = false;
        int counter = 0;
        while (!done && counter <= 5)
        {
            original = base.GetPosition();
            original.y = spawnBoxCenter.y + Random.Range(-spawnBoxSize.y / 2f, spawnBoxSize.y / 2f);
            // check if something is at "original"
            Transform closest = null;
            float smallestDistance = float.MaxValue;

            foreach (Transform child in transform)
            {
                if (Vector3.Distance(original, child.position) < smallestDistance)
                {
                    closest = child;
                    smallestDistance = Vector3.Distance(original, child.position);
                }
            }

            if (smallestDistance <= .5f)
            {
                counter++;
            }
            else
            {
                done = true;
            }
        }
        if (!done)
        {
            Vector3 v = base.GetPosition();
            v.y = spawnBoxCenter.y + Random.Range(-spawnBoxSize.y / 2f, spawnBoxSize.y / 2f);
            return v;
        }
        return new Vector3(original.x, original.y, original.z);
    }
    
    public static void DisableForSeconds(Obstacle o, float f)
    {
        //Debug.Log("Disabling " + o.name);
        o.PlayDisabledSound();
        o.gameObject.SetActive(false);

        Dummy d = new GameObject().AddComponent<Dummy>();

        if(d != null)
        {
            d.StartCoroutine(Wait(f, () => { if (o != null) { o.gameObject.SetActive(true); o.EnableObstacle(); Destroy(d); } }));
        }
    }

    private static IEnumerator Wait(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    //private IEnumerator SetActiveAfterSeconds(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    if (gameObject != null)
    //    {
    //        gameObject.SetActive(true);
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        Obstacle o = other.gameObject.GetComponent<Obstacle>();
        if (o != null)
        {
            o.Destruct();
        }
    }
}
