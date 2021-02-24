using UnityEngine;
using UnityEngine.EventSystems;

public class AnimateOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator animator = null;
    [SerializeField] AudioMod audioMod = null;
    private void OnMouseOver()
    {
        if (animator != null)
        {
            animator.enabled = true;
            if(audioMod != null)
            {
                audioMod.PlayAudioClip(1);
            }
        }
    }

    private void OnMouseExit()
    {
        //if(animator != null)
        //{
        //    animator.enabled = false;
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseOver();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (animator != null)
        //{
        //    animator.enabled = false;
        //}
    }
    
}
