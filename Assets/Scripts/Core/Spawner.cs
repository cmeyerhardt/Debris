using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{

    public GameObjectEvent spawnEvent = new GameObjectEvent();
    [SerializeField] internal ScoreTracker scoreTracker = null;

    [SerializeField] public GameObject toSpawn = null;
    [SerializeField] public Transform parent = null;
    public Coroutine spawningRoutine = null;

    [Header("Spawn Location")]
    [SerializeField] public Vector3 spawnBoxSize = new Vector3();
    [SerializeField] public Vector3 spawnBoxCenter = new Vector3();

    public int MaxSpawnBoxWidth { get { return 13; } } //todo -- calcualte max width given camera viewport/resolution of game window
    
    public float spawnInterval = 2f;
    float interval = 0f;
    public static bool pause = false;

    [Header("Object Transform Variation")]
    [SerializeField] bool varyRotation = true;
    [SerializeField] public bool varySize = false;
    [SerializeField] Vector3 minSize = new Vector3();
    [SerializeField] Vector3 maxSize = new Vector3();
    [SerializeField] public float varySizeDelay = 0f;

    [Header("Debug")]
    [SerializeField] public Color gizmoColor = Color.white;

    internal virtual void Start()
    {
        if(varySize)
        {
            Invoke("SetVarySize", varySizeDelay);
        }
        interval = 0f;

        //if (spawningRecursive)
        //{
        //    //todo -- change to coroutine
        //    Invoke("Spawn", spawnInterval);
        //}
    }


    private void OnDrawGizmos()
    {
        //Display spawning area
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(spawnBoxCenter, spawnBoxSize);
    }

    public virtual void Update()
    {
        if(pause) { return; }
        if(interval > spawnInterval)
        {
            interval = 0f;
            _ = Spawn();
        }
        else
        {
            interval += Time.deltaTime;
        }
    }

    //OBJECT
    public virtual GameObject Spawn()
    {
        //Debug.Log(gameObject.name + " Spawn");
        GameObject o = SpawnObject();

        //Transform variation
        if (varySize)
        {
            VarySize(o);
        }
        if(varyRotation)
        {
            VaryRotation(o);
        }

        spawnEvent.Invoke(o);

        //if (spawningRecursive)
        //{
        //    //Debug.Log("Spawn Recursive TRUE");
        //    //todo -- doafter coroutine
        //    Invoke("Spawn", spawnInterval);
        //}
        return o;
    }

    public virtual GameObject SpawnObject()
    {
        Vector3 position = GetPosition();
        return Instantiate(toSpawn, position, Quaternion.identity, parent);
    }

    public virtual Vector3 GetPosition()
    {
        return new Vector3(
                Random.Range(-spawnBoxSize.x / 2f, spawnBoxSize.x / 2f),
                spawnBoxCenter.y,
                spawnBoxCenter.z);// (Random.Range(0, 2) >= 1) ? (spawnBoxSize.z / 2f).PlusOrMinus() : 0f); //randomly in center or to either (+/-) side
    }



    public void SetVarySize()
    {
        varySize = true;
    }

    public virtual void VarySize(GameObject o)
    {
        if(o != null)
        {
            o.transform.localScale = Random.Range(minSize.x, maxSize.x).ToVector3();// new Vector3(Random.Range(minSize.x, maxSize.x), Random.Range(minSize.y, maxSize.y), Random.Range(minSize.z, maxSize.z));
        }
    }

    public virtual void VaryRotation(GameObject o)
    {
        //if (o != null)
        //{
        //    o.transform.forward = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        //}
    }
    


    // SPAWN RATE 
    public void IncreaseSpawnRate()
    {
        spawnInterval *= 1.1f;
    }

    public void DecreaseSpawnRate()
    {
        spawnInterval *= .9f;
    }

    //public void StopSpawningTemp(float waitTime)
    //{
    //    if(!pause)
    //    {
    //        pause = true;
    //        StopAllCoroutines();
    //        Invoke("SetSpawningOn", waitTime);
    //    }
    //}

    public void SetSpawningOff()
    {
        pause = true;
        StopAllCoroutines();
    }

    public void SetSpawningOn()
    {
        pause = false;
        //Spawn();
    }

    public void DestroyAllSpawns()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void IncreaseSpawnWidth()
    {
        spawnBoxSize.x = Mathf.Clamp(spawnBoxSize.x + 1f, 1f, MaxSpawnBoxWidth);
    }

    public void SetWidth(float width)
    {
        spawnBoxSize.x = width;
    }
}
