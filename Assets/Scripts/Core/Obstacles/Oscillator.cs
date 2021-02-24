using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [System.Serializable]
    public class OscillatorPattern
    {
        public SnapAxis axisOfMovement;
        public float period;
        public float magnitude;

        public OscillatorPattern(SnapAxis axisOfMovement, float period, float magnitude)
        {
            this.axisOfMovement = axisOfMovement;
            this.period = period;
            this.magnitude = magnitude;
        }
    }

    [SerializeField] OscillatorPattern[] patterns = new OscillatorPattern[3]
    {
        new OscillatorPattern(SnapAxis.X, 0f, 0f),
        new OscillatorPattern(SnapAxis.Y, 0f, 0f),
        new OscillatorPattern(SnapAxis.Z, 0f, 0f),
    };

    const float tau = Mathf.PI * 2f;

    public bool oscillate = true;
    public bool initialRandom = true;

    Vector3 startingPosition = new Vector3();    //coordinates for the object's starting position
    float movementFactor = 0f;                   //0 = not moved, 1 = 100% movement

    float randomSeed = 0f;
    float lifeTime = 0f;

    private void Awake()
    {
        randomSeed = .5f + Random.Range(-.15f, .15f);
        startingPosition = transform.position;

        //if(patterns.Length == 0)
        //{
        //    patterns = new OscillatorPattern[3];
        //    patterns[0] = new OscillatorPattern(SnapAxis.X, 0f, 0f);
        //    patterns[1] = new OscillatorPattern(SnapAxis.Y, 0f, 0f);
        //    patterns[2] = new OscillatorPattern(SnapAxis.Z, 0f, 0f);
        //}
        
        if (initialRandom)
        {
            int choice = Random.Range(0, patterns.Length - 1);
            InitializeAxis(choice);
        }
    }

    private void InitializeAxis(int choice)
    {
        patterns[choice].period = 1f;
        patterns[choice].magnitude = 1f;
    }

    void Update()
    {
        if(!oscillate) { return; }

        lifeTime += Time.deltaTime;

        if (patterns != null && patterns.Length > 0)
        {
            Vector3 offsetThisFrame = Vector3.zero;

            foreach (OscillatorPattern pattern in patterns)
            {
                if(pattern.period <= Mathf.Epsilon) { continue; }

                movementFactor = Mathf.Sin(/*(Time.time + randomSeed)*/lifeTime / pattern.period * tau) / 2f + .5f; //(+.5)Need range to be [0,1]

                switch (pattern.axisOfMovement)
                {
                    case SnapAxis.X:
                        offsetThisFrame.x = pattern.magnitude * movementFactor;
                        break;
                    case SnapAxis.Y:
                        offsetThisFrame.y = pattern.magnitude * movementFactor;
                        break;
                    case SnapAxis.Z:
                        offsetThisFrame.z = pattern.magnitude * movementFactor;
                        break;
                    default:
                        break;
                }
            }
            transform.position = startingPosition + offsetThisFrame;
        }
    }

    public void ShiftStartingPositionUp()
    {
        startingPosition = new Vector3(startingPosition.x, startingPosition.y + 1f, startingPosition.z);
    }

    public void ShiftStartingPositionDown()
    {
        startingPosition = new Vector3(startingPosition.x, startingPosition.y - 1f, startingPosition.z);
    }

    public void IncreaseSpeed()
    {
        if(patterns.Length <= 0) { return; }
        
        int choice = Random.Range(0, patterns.Length - 1);
        OscillatorPattern pattern = patterns[choice];

        if (pattern != null)
        {
            if (pattern.period == 0f || pattern.period == 0f)
            {
                InitializeAxis(choice);
                return;
            }
            pattern.period *= 2f;
        }
    }

    public void IncreaseMagnitude()
    {
        if (patterns.Length <= 0) { return; }

        int choice = Random.Range(0, patterns.Length - 1);
        OscillatorPattern pattern = patterns[choice];

        if (pattern != null)
        {
            if (pattern.period == 0f || pattern.period == 0f)
            {
                InitializeAxis(choice);
                return;
            }
            pattern.magnitude *= 2f;
        }
    }

    public void DecreaseSpeed()
    {
        if (patterns.Length <= 0) { return; }

        int choice = Random.Range(0, patterns.Length - 1);
        OscillatorPattern pattern = patterns[choice];

        if (pattern != null)
        {
            if (pattern.period == 0f || pattern.period == 0f)
            {
                InitializeAxis(choice);
                return;
            }
            pattern.period *= .75f;
        }
    }

    public void DecreaseMagnitude()
    {
        if (patterns.Length <= 0) { return; }

        int choice = Random.Range(0, patterns.Length - 1);
        OscillatorPattern pattern = patterns[choice];

        if (pattern != null)
        {
            if (pattern.period == 0f || pattern.period == 0f)
            {
                InitializeAxis(choice);
                return;
            }
            pattern.magnitude *= .75f;
        }
    }
}
