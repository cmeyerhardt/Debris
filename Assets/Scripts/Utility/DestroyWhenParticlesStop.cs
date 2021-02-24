using UnityEngine;

public class DestroyWhenParticlesStop : MonoBehaviour
{
    [SerializeField] GameObject toDestroy = null;
    ParticleSystem ps = null;

    private void Awake()
    {
        if(ps == null)
        {
            ps = GetComponent<ParticleSystem>();
        }

        if (ps == null)
        {
            Destroy(gameObject);
        }

        if(toDestroy == null)
        {
            toDestroy = gameObject;
        }

    }

    void Update()
    {
        if (ps != null)
        {
            if (!ps.isPlaying)
            {
                if (toDestroy != null)
                {
                    Destroy(toDestroy);
                }
            }
        }
    }
}
