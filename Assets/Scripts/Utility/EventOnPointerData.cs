using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventOnPointerData : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onPointerDownEvent = null;
    public UnityEvent onPointerUpEvent = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDownEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUpEvent.Invoke();
    }
}
