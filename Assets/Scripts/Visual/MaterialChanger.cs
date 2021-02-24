using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] Material materialToAssign = null;

    private void OnEnable()
    {
        if(materialToAssign == null) { Destroy(this); }

        foreach (Transform child in transform)
        {
            if(child == null) { return; }

            MeshRenderer m = child.GetComponent<MeshRenderer>();
            if(m != null)
            {
                m.material = materialToAssign;
                m.SetColor(new Color(Random.Range(.85f, 1f), Random.Range(.85f, 1f), Random.Range(.85f, 1f), 1f));
            }

            Collider c = child.GetComponent<Collider>();
            if (c != null)
            {
                c.isTrigger = true;
            }
        }

        Destroy(this);
    }
}
