using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TransformChildSwitcher switcher_Transform = null;
    [SerializeField] AnimatorChildSwitcher switcher_Animator = null;
    [SerializeField] GameObject mouseOverGraphic = null;
    [SerializeField] AudioMod audioMod = null;

    private void OnMouseOver()
    {
        //activeGraphic.SetActive(true);
        if(mouseOverGraphic!= null)
            mouseOverGraphic.SetActive(true);

        int siblingIndex = transform.GetSiblingIndex();
        if (switcher_Transform != null) 
            switcher_Transform.ActivateChild(siblingIndex);
        if (switcher_Animator != null)
            switcher_Animator.ActivateChild(siblingIndex);

        if(audioMod!= null)
        {
            audioMod.PlayAudioClip(0);
        }
    }

    private void OnMouseExit()
    {
        if (mouseOverGraphic != null)
            mouseOverGraphic.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseOver();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }
}
