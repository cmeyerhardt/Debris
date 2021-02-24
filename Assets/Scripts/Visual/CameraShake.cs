using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Animator animator = null;

    private void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (animator == null)
        {
            //addcomponent animator
            //resources load the controller
            Debug.Log("Animator cannot be found for " + gameObject.name);
        }
    }

    public void Shake()
    {
        if(animator != null)
        {
            animator.ResetTrigger("Shake");
            animator.SetTrigger("Shake");
        }
    }
}
