using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] Transform parentToDestruct = null;

    private void OnMouseDown()
    {
        if(Time.timeScale < 1f) { return; }
        ////Destroy(this);
        Destruct(true);
        enabled = false;
    }

    public void Destruct(bool b)
    {
        //transform.parent = null;
        if (b) { Destroy(this); return; }
        //transform.GetChild(0).parent = null;

        AddRigidBodiesToAllChildren();
        //switch (Random.Range(0, 3))
        //{
        //    case 1:
        //    //AddRigidBodiesToNewGroups();
        //    //break;
        //    case 2:
        //    //AddRigidBodiesToImmediateChildrenOnly(transform.GetChild(0));
        //    //break;
        //    default:
        //        AddRigidBodiesToAllChildren();
        //        break;
        //}

        if(parentToDestruct != null)
        {
            foreach (Transform child in parentToDestruct)
            {
                if (child != null)
                {
                    child.parent = null;
                    Destroy(child.gameObject, 5f);
                }
            }
        }
        else
        {
            Transform t = transform.GetChild(0);
            if (t != null)
            {
                foreach (Transform child in t)
                {
                    if (child != null)
                    {
                        child.parent = null;
                        Destroy(child.gameObject, 5f);
                    }
                }
            }
        }

        //Destroy(gameObject, 5.5f);
    }

    private void AddRigidBodiesToNewGroups()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        switch (Random.Range(0, 2))
        {
            case 0:
                AddRigidBodiesToImmediateChildrenOnly(transform.GetChild(0));
                break;
            default:
                AddRigidBodiesToEachTransformElement(allChildren);
                break;
        }
    }



    private void AddRigidBodiesToImmediateChildrenOnly(Transform parent)
    {
        if(parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform t = parent.GetChild(i);
                if(t != null)
                {
                    AddActiveRigidBody(t.gameObject);
                }
            }
        }

    }

    private void AddRigidBodiesToAllChildren()
    {
        AddRigidBodiesToEachTransformElement(transform.GetChild(0));
    }

    private void AddRigidBodiesToEachTransformElement(Transform _transform)
    {
        if(_transform != null)
        {
            foreach (Transform child in _transform)
            {
                AddActiveRigidBody(child.gameObject);
            }
        }

    }

    private void AddRigidBodiesToEachTransformElement(Transform[] transformList)
    {
        if(transformList != null && transformList.Length > 0)
        {
            foreach (Transform node in transformList)
            {
                AddActiveRigidBody(node.gameObject);
            }
        }
    }


    private void AddActiveRigidBody(GameObject _child)
    {
        if(_child == null) { return; }
        Rigidbody rb = _child.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = _child.AddComponent<Rigidbody>();
        }

        if (rb != null)
        {
            Block b = rb.GetComponent<Block>();
            if(b != null)
            {
                b.rb = rb;
            }

            Collider c = rb.GetComponent<Collider>();
            if (c != null)
            {
                c.isTrigger = false;
            }

            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
