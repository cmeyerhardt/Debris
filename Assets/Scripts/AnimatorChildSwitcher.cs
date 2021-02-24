using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorChildSwitcher : MonoBehaviour
{
    Animator activeChildAnimator = null;

    private void Awake()
    {
        if (transform.childCount > 0)
        {
            //activate first child
            ActivateChild(0);

            if (transform.childCount > 1)
            {
                //deactivate all other childern
                for (int i = 1; i < transform.childCount; i++)
                {
                    //Verify referencing
                    Transform child = transform.GetChild(i);
                    if (child == null) { return; }
                    if (child.gameObject == null) { return; }

                    Animator anim = child.GetComponent<Animator>();
                    if (anim == null) { return; }
                    anim.enabled = false;
                }
            }
        }
    }

    public void ActivateChild(int childSiblingIndex)
    {
        if (transform.childCount <= childSiblingIndex) { return; }

        //Verify referencing
        Transform child = transform.GetChild(childSiblingIndex);
        if (child == null) { return; }
        if (child.gameObject == null) { return; }

        Animator anim = child.GetComponent<Animator>();
        if (anim == null) { return; }

        //deactivate current active child
        if (activeChildAnimator != null)
        {
            activeChildAnimator.enabled = false;
            activeChildAnimator = null;
        }

        //set new active child
        activeChildAnimator = anim;
        activeChildAnimator.enabled = true;
    }
}
