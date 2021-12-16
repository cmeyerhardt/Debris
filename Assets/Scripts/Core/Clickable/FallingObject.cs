using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FallingObject : ClickForPoints
{
    [Header("3D")]
    [SerializeField] GameObject freezeFX = null;
    [SerializeField] GameObject landFX = null;
    [SerializeField] GameObject dragFX = null;
    [SerializeField] Rigidbody rb = null;

    public static bool passive = false;
    public static bool freeze = false;
    public static new bool destroy = false;
    
    // Cache
    private bool floor = false;
    //float counter = .1f;
    //ClickForPoints2D spawn2D = null;

    public void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 25f;
            rb.useGravity = true;
            rb.isKinematic = false;
        }

    }

    private void OnMouseDown()
    {
        if (Time.timeScale < 1f) { return; }

        SuccessfullyClick();
    }

    private void OnMouseOver()
    {
        if (Time.timeScale < 1f) { return; }

        if (passive || Input.GetKeyDown(KeyCode.Space))
        {
            SuccessfullyClick();
        }
    }

    public void Update()
    {
        if (Time.timeScale < 1f) { return; }
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        //todo: move this to base?
        if (freeze != rb.isKinematic && !pointsEarned)
        {
            rb.isKinematic = freeze;
            if (freeze && freezeFX != null)
            {
                Instantiate(freezeFX, transform.position, Quaternion.identity, null);
            }
        }

        //todo: move this to base?
        if(destroy)
        {
            SuccessfullyClick();
        }

        // objects way off screen should be destroyed right away
        if (!gameObject.IsOnScreen(Camera.main) && Vector3.Distance(transform.position, Vector3.zero) > 100f)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (pointsEarned) //metal
        {
            if (collision.gameObject.tag == "Surface")
            {
                ImpactFloor();
            }
            else
            {
                Obstacle o = collision.gameObject.GetComponent<Obstacle>();
                if (o != null)
                {

                    if(o.DisableObstacle())
                    {
                        ObstacleSpawner.DisableForSeconds(o, 5f);
                    }
                }
                else
                {
                    FallingObject c = collision.gameObject.GetComponent<FallingObject>();
                    if (c != null)
                    {
                        c.SuccessfullyClick();
                    }
                }
            }

        }
        else //wooden thing
        {
            if (collision.gameObject.tag == "Surface" || collision.gameObject.tag == "Obstacle")
            {
                ImpactFloor();
            }
            else
            {
                Block b = collision.gameObject.GetComponent<Block>();
                if (b != null)
                {
                    if (b.isGrounded)
                    {
                        ImpactFloor();
                    }
                    else
                    {
                        if(audioMod != null)
                        {
                            audioMod.PlayAudioClip(2);
                        }
                    }
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (pointsEarned) //metal
        {
            if (other.gameObject.tag == "Surface")
            {
                if (pointsEarned && landFX != null)
                {
                    Instantiate(landFX, other.ClosestPointOnBounds(transform.position), Quaternion.identity, null);
                    landFX = null;
                    dragFX.SetActive(true);
                }
                foreach(Collider c in GetComponentsInChildren<Collider>())
                {
                    c.isTrigger = false;
                }
                ImpactFloor();
            }
            else if (other.gameObject.tag == "Obstacle")
            {
                Obstacle o = other.gameObject.GetComponent<Obstacle>();
                if (o != null)
                {
                    if (o.DisableObstacle())
                    {
                        if(audioMod != null)
                        {
                            audioMod.PlayAudioClip(4);
                        }

                        ObstacleSpawner.DisableForSeconds(o, 5f);
                    }
                }
            }
            else
            {
                FallingObject c = other.GetComponent<FallingObject>();
                if (c != null)
                {
                    c.SuccessfullyClick();
                }
            }

        }
    }

    public override void SuccessfullyClick()
    {
        if(rb != null && gameObject != null)
        {
            if (!pointsEarned &&  PowerUpButton.activePowerUps.Count > 0 && Random.Range(0, Random.Range(85, 95)) < 2)
            {
                PowerUp powerUp = PowerUpButton.DeterminePowerUp();
                ClickForPoints2D spawn = ObjectSpawner.GetSpawn(powerUp);
                spawn = Instantiate(spawn, null);
                spawn.Reposition(transform.position);
                spawn.transform.parent = null;
                spawn.gameObject.SetActive(true);
            }

            rb.isKinematic = false;
            rb.AddForce(Vector3.down * 200f, ForceMode.Acceleration);

            base.SuccessfullyClick();

            MaterialChanger m = GetComponentInChildren<MaterialChanger>();
            if (m != null)
            {
                m.enabled = true;
            }

            Collider c = GetComponent<Collider>();
            if(c != null)
            {
                Destroy(c);
            }


        }
    }

    public override void ImpactFloor()
    {
        if (!gameObject.IsOnScreen(Camera.main)) { Destroy(gameObject); return; }
        if (floor) { return; }
        floor = true;

        if (!pointsEarned)
        {
            intEvent.Invoke(-numPoints);
        }
        else
        {
            //PrepareObject for moving on ground as metallic thing
            if (pointsEarned && landFX != null)
            {
                Instantiate(landFX, transform.position, Quaternion.identity, null);
                landFX = null;
                dragFX.SetActive(true);
            }
            rb.constraints = RigidbodyConstraints.None;
            if (GetComponent<Magnetic>() == null)
            {
                gameObject.AddComponent<Magnetic>();
            }
        }
        base.ImpactFloor();
    }

    //public override void Freeze(float duration = 5f)
    //{
    //    if (!frozen && rb != null)
    //    {
    //        rb.isKinematic = true;
    //        Instantiate(freezeFX, transform.position, Quaternion.identity, null);
    //        base.Freeze(duration);
    //    }
    //}

    //public override void UnFreeze()
    //{
    //    rb.isKinematic = false;
    //    base.UnFreeze();
    //}

    public void BecomeMolten()
    {

    }
}

