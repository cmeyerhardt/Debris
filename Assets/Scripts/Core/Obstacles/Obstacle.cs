using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //[SerializeField] MeshRenderer meshRenderer = null;
    [SerializeField] Transform toScale = null;
    [SerializeField] MeshRenderer ghost = null;
    [SerializeField] AudioMod audioMod = null;
    [SerializeField] Animator animator = null;
    [SerializeField] BoxCollider boxCollider = null;

    [SerializeField] AreaClick areaClick = null;
    [SerializeField] Oscillator oscillator = null;
    [SerializeField] GameObject destroyFX = null;
    [SerializeField] Destructable destructable = null;

    //private void OnMouseDown()
    //{
    //    if(Time.timeScale < 1f) { return; }

    //    if(areaClick != null)
    //    {
    //        areaClick.ClickInArea();
    //    }
    //}

    private void Update()
    {
        if (!gameObject.IsOnScreen(Camera.main)) { Destruct(); return; }
    }

    public void Destruct()
    {
        if (destroyFX != null)
        {
            Instantiate(destroyFX, transform.position, Quaternion.identity, null);
        }
        //destructable.transform.parent = null;
        //if(destructable != null)
        //{
        //    destructable.Destruct(false);
        //}
        Destroy(gameObject);
    }

    public AreaClick GetAreaClick()
    {
        return areaClick;
    }

    public void SetAreaClick(AreaClick a)
    {
        areaClick = a;
    }

    public void PlaySound()
    {
        if(audioMod != null)
        {
            audioMod.PlayAudioClip(0);
        }
    }

    public Oscillator GetOscillator()
    {
        return oscillator;
    }

    public void SetOscillator(Oscillator o)
    {
        oscillator = o;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Surface")
        {
            Debug.Log("Destructing against: " + collision.gameObject.name);
            Destruct();
        }
    }

    public bool IncreaseSize()
    {
        //if (meshRenderer.transform.localScale.y >= 3f)
        if (toScale.localScale.y >= 3f)
        {
            //todo -- animation to indicate obstacle being destroyed
            destructable.Destruct(true);
            return false;
            //Destroy(gameObject);
        }

        //if (meshRenderer != null)
        //{
        //    meshRenderer.transform.localScale *= 1.5f;
        //}
        if (toScale != null)
        {
            toScale.localScale *= 1.2f;
        }
        if(ghost != null)
        {
            ghost.transform.localScale *= 1.2f;
        }
        if(audioMod != null)
        {
            audioMod.PlayAudioClip(0);
        }
        if (boxCollider != null)
        {
            boxCollider.size *= 1.2f;
        }

        SetAnimatorState(0);
        return true;
    }


    public bool DecreaseSize()
    {
        //if (meshRenderer.transform.localScale.y <= .1f)
        if (toScale.localScale.y <= .1f)
        {
            //todo -- animation to indicate obstacle being destroyed
            destructable.Destruct(true);
            return false;
            //Destroy(gameObject);
        }

        //if (meshRenderer != null)
        //{
        //    meshRenderer.transform.localScale *= .5f;
        //}
        if (toScale != null)
        {
            toScale.localScale *= .5f;
        }
        if (ghost != null)
        {
            ghost.transform.localScale *= .5f;
        }
        if (audioMod != null)
        {
            audioMod.PlayAudioClip(0);
        }
        if(boxCollider != null)
        {
            boxCollider.size *= .5f;
        }

        SetAnimatorState(0);
        return true;
    }

    public bool MoveUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);


        if (oscillator != null)
        {
            oscillator.ShiftStartingPositionUp();
        }

        if (ghost != null && ghost.transform.parent != transform)
        {
            ghost.transform.position = transform.position;
        }

        if(audioMod != null)
        {
            audioMod.PlayAudioClip(0);
        }

        SetAnimatorState(0);
        return true;
    }

    public bool MoveDown()
    {
        Debug.Log("MovingDown");
        transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        if(oscillator != null)
        {
            oscillator.ShiftStartingPositionDown();
        }

        if (ghost != null && ghost.transform.parent != transform)
        {
            ghost.transform.position = transform.position;
        }
        if (audioMod != null)
        {
            audioMod.PlayAudioClip(0);
        }
        SetAnimatorState(0);
        return true;
    }

    private void OnDestroy()
    {
        if (ghost != null && ghost.transform.parent != transform)
            Destroy(ghost.gameObject);
    }

    public void PlayDisabledSound()
    {
        if (audioMod != null)
        {
            audioMod.PlayAudioClip(1);
        }
    }

    public bool DisableObstacle()
    {
        if (audioMod != null)
        {
            audioMod.PlayAudioClip(1);
        }
        if (ghost != null) 
        {
            ghost.transform.parent = null;
        }
        return true;
    }

    public void EnableObstacle()
    {
        if(ghost != null)
        {
            ghost.transform.parent = transform;
        }

        //if (audioMod != null)
        //{
        //    audioMod.PlayAudioClip(0);
        //}

        SetAnimatorState(0);
    }

    public void PlayArrivalAnim()
    {
        SetAnimatorState(0);
    }

    public void SetAnimatorToIdle()
    {
        SetAnimatorState(1);
    }

    public void SetAnimatorState(int i)
    {
        if(animator != null)
        {
            animator.SetInteger("State", i);
        }
    }

    //public void DisableForSeconds(float f)
    //{

    //    StartCoroutine(SetActiveAfterSeconds(f));
    //    gameObject.SetActive(false);
    //}

    //private IEnumerator SetActiveAfterSeconds(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    gameObject.SetActive(true);
    //}
    
}
