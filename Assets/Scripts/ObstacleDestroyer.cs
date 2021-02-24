using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
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
        if (destructed) { return; }
        Debug.Log("Mouse Down obstacle destroyer");
        Vector3 _position = transform.position;

        if (radiusIndicator != null && radiusIndicatorGO != null)
        {
            radiusIndicator.color = Color.cyan;
            radiusIndicatorGO.SetActive(true);
        }

        //if (fx != null)
        //{
        //    Instantiate(fx, (_position == null) ? transform.position : (Vector3)_position, Quaternion.identity, null);
        //}

        Collider[] collidersHit = Physics.OverlapSphere(_position, radius);
        foreach (Collider collider in collidersHit)
        {
            Obstacle c = collider.GetComponent<Obstacle>();
            if (c != null)
            {
                c.Destruct();
            }
        }

        Destruct(_position);
        //Destroy(gameObject);
    }

    private void Update()
    {
        if (!gameObject.IsOnScreen(Camera.main)) { Destroy(gameObject); return; }

        if (Fly)
        {
            transform.position += transform.position.normalized * Time.deltaTime;
        }
    }

    private void Destruct(Vector3? _position = null)
    {
        if (destructed) { return; }
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
        Destroy(gameObject, .5f);
    }

    private void OnMouseOver()
    {
        radiusIndicatorGO.SetActive(true);
    }

    private void OnMouseExit()
    {
        radiusIndicatorGO.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (destructed) { return; }
        destructed = true;
        DontClickMe dontClickMe = collision.gameObject.GetComponent<DontClickMe>();

        if (dontClickMe != null)
        {
            dontClickMe.Destruct(transform.position);
        }
    }
}
