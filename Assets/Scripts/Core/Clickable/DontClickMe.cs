using UnityEngine;

public class DontClickMe : MonoBehaviour
{
    [SerializeField] GameObject fx = null;
    [SerializeField] GameObject radiusIndicatorGO = null;
    [SerializeField] SpriteRenderer radiusIndicator = null;
    [SerializeField] Destructable destructable = null;
    bool destructed = false;
    public static bool Fly = false;
    [SerializeField] float radius = 2.5f;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}
    private void Awake()
    {
        radiusIndicatorGO.transform.localScale = radius.ToVector3();
    }

    private void OnMouseDown()
    {
        if(destructed) { return; }
        Vector3 _position = transform.position;

        Collider[] collidersHit = Physics.OverlapSphere(_position, radius);
        if(collidersHit.Length > 0)
        {
            foreach (Collider collider in collidersHit)
            {
                ClickForPoints c = collider.GetComponent<ClickForPoints>();
                if (c != null)
                {
                    c.ImpactFloor();
                }
            }
        }

        if (radiusIndicator != null && radiusIndicatorGO != null)
        {
            radiusIndicator.color = Color.yellow;
            radiusIndicatorGO.SetActive(true);
        }

        if (fx != null)
        {
            Instantiate(fx, (_position == null) ? transform.position : (Vector3)_position, Quaternion.identity, null);
        }

        Destruct();
        //Destroy(gameObject);
    }

    private void OnMouseOver()
    {
        radiusIndicatorGO.SetActive(true);
    }

    private void OnMouseExit()
    {
        radiusIndicatorGO.SetActive(false);
    }

    private void Update()
    {
        if (!gameObject.IsOnScreen(Camera.main)) { Destroy(gameObject); return; }

        if(Fly)
        {
            transform.position += transform.position.normalized * Time.deltaTime;
        }
    }

    public void Destruct(Vector3? _position = null)
    {
        if(destructed) { return; }
        destructed = true;
        if (fx != null)
        {
            Instantiate(fx, (_position == null) ? transform.position : (Vector3)_position, Quaternion.identity, null);
        }

        Collider c = GetComponent<Collider>();
        if (c != null)
        {
            c.enabled = false;
        }

        destructable.Destruct(false);
        
        StopAllCoroutines();
        Destroy(gameObject, 1f);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (destructed) { return; }
    //    destructed = true;
    //    FallingObject f = collision.gameObject.GetComponent<FallingObject>();
    //    if (f != null)
    //    {
    //        if (!f.pointsEarned)
    //        {
    //            f.ImpactFloor();
    //        }
    //        Destruct();
    //    }
    //}
}
