using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChildSwitcher : MonoBehaviour
{
    GameObject activeChildGO = null;

    private void Awake()
    {
        if(transform.childCount > 0)
        {
            //activate first child
            ActivateChild(0);

            if(transform.childCount > 1)
            {
                //deactivate all other childern
                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    public void ActivateChild(int childSiblingIndex)
    {
        //return if no children
        if (transform.childCount <= childSiblingIndex) { return; }

        Transform child = transform.GetChild(childSiblingIndex);
        if(child == null) { return; }
        if(child.gameObject == null) { return; }

        //deactivate current active child
        if (activeChildGO != null)
        {
            activeChildGO.SetActive(false);
            activeChildGO = null;
        }

        //set new active child
        activeChildGO = child.gameObject;
        activeChildGO.SetActive(true);
    }
}
