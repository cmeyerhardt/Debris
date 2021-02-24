using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsOnMouseOver : MonoBehaviour
{
    public UnityEvent mouseOverEvent = null;
    public UnityEvent mouseExitEvent = null;

    private void OnMouseOver()
    {
        mouseOverEvent.Invoke();
    }

    private void OnMouseExit()
    {
        mouseExitEvent.Invoke();
    }
}
