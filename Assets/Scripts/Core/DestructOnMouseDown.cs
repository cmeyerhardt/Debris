using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructOnMouseDown : MonoBehaviour
{
    [SerializeField] Destructable destructable = null;
    [SerializeField] AudioMod audioMod = null;
    [SerializeField] AnimateOnMouseOver animateOnMO = null;
    [SerializeField] Animator animator = null;

    private void OnMouseDown()
    {
        if(destructable != null)
        {
            destructable.Destruct(false);
            Destroy(destructable, 2f);
        }
        if(audioMod != null)
        {
            audioMod.PlayAudioClip(0);
            Destroy(audioMod, 2f);
        }
        if(animateOnMO != null)
        {
            Destroy(animateOnMO);
        }
        if (animator != null)
        {
            Destroy(animator);
        }
        Destroy(this);
    }
}
