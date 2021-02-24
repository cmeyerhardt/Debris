using UnityEngine;

public class UnSyncAnimator : MonoBehaviour
{
    Animator animator = null;

    private void OnEnable()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        Invoke("PlayAnimator", Random.Range(0f, .5f));
    }

    void PlayAnimator()
    {
        if(animator != null)
        {
            animator.enabled = true;
        }
    }
}
