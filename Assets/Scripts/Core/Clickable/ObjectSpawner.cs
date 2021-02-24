using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum FallingObjectType { A, B, C, D, E, F }

public class ObjectSpawner : Spawner
{
    public UnityEvent unsuccessful;
    public UnityEvent successful;
    public UnityEvent cameraShake;

    //[SerializeField] DisplayCase displayCase = null;
    //todo -- prefab all models and load from resources initially, including bad shapes
    [SerializeField] GameObject[] models = null;
    public static Dictionary<PowerUp, ClickForPoints2D> powerUpSpawns = new Dictionary<PowerUp, ClickForPoints2D>();

    int numModelsSpawn = 1;
    public static bool stopSpawning = false;


    public static ClickForPoints2D GetSpawn(PowerUp powerUp)
    {
        if(powerUpSpawns.ContainsKey(powerUp))
        {
            return powerUpSpawns[powerUp];
        }
        return null;
    }
    
    private void Awake()
    {
        ClickForPoints2D[] spawns2D = Resources.LoadAll<ClickForPoints2D>("2DSpawns");
        foreach(ClickForPoints2D spawn in spawns2D)
        {
            if(!powerUpSpawns.ContainsKey(spawn.powerUp))
            {
                powerUpSpawns.Add(spawn.powerUp, spawn);
            }
            else
            {
                Debug.LogWarning("Duplicate PowerUp GameObject attempted insert into collection: " + spawn.powerUp + "(" + spawn.name + ")");
            }
        }
    }

    private new void Update()
    {
        if(stopSpawning) { return; }

        base.Update();
    }

    public override GameObject Spawn()
    {
        GameObject g = base.Spawn();
        ClickForPoints o = g.GetComponent<ClickForPoints>();
        if(o != null)
        {
            o.boolEvent.AddListener(Destroyed);
            o.impact.AddListener(ShakeCamera);


            // Vary Model
            int i = Random.Range(0, Mathf.Min(numModelsSpawn, models.Length));
            if(models[i] != null)
            {
                o.type = (FallingObjectType)i;
                GameObject m = Instantiate(models[i], o.transform.position, Quaternion.identity, o.displayRoot);
                o.displayRoot.transform.forward = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }

            //o.boolEvent.AddListener((bool b) => { if (displayCase.isActiveAndEnabled) { displayCase.DisplayThing(i); } });
            o.intEvent.AddListener(scoreTracker.ProcessScoreChange);

            //spawnEvent.Invoke(o);
            //spawnEvent.Invoke(g);
        }
        return o.gameObject;
    }

    public void ShakeCamera()
    {
        cameraShake.Invoke();
    }

    public override Vector3 GetPosition()
    {
        Vector3 original = new Vector3();
        bool done = false;
        int counter = 0;
        while (!done && counter <= 5)
        {
            original = base.GetPosition();

            // check if something is at "original"
            Transform closest = null;
            float smallestDistance = float.MaxValue;
            
            foreach(Transform child in transform)
            {
                if(Vector3.Distance(original, child.position) < smallestDistance)
                {
                    closest = child;
                    smallestDistance = Vector3.Distance(original, child.position);
                }
            }

            if(smallestDistance <= 2f)
            {
                counter++;
            }
            else
            {
                done = true;
            }
        }
        if(!done)
        {
            return new Vector3(1000f, 1000f, 1000f);
        }
        return new Vector3(original.x, original.y, original.z);
    }
    

    public override void VarySize(GameObject o)
    {
        if (o != null) 
        {
            float size = o.transform.position.z / (spawnBoxSize.z);
            o.transform.localScale = Vector3.one + size.ToVector3();
        }
    }

    public void Destroyed(bool success)
    {
        if(success)
        {
            successful.Invoke();
        }
        else
        {
            unsuccessful.Invoke();
        }
    }

    public void IncreaseNumberOfShapes()
    {
        numModelsSpawn = Mathf.Clamp(numModelsSpawn + 1, 0, models.Length);
    }



    //public void Spawn2D(Vector3 position)
    //{
    //    ClickForPoints2D o = Instantiate(toSpawn2D, position, Quaternion.identity, transform);
    //    o.Reposition(position);
    //    //o.clickedEvent.AddListener(() => { powerUpTracker.StorePowerUp(); });
    //}


}
