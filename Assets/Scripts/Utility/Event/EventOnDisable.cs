using UnityEngine;
using UnityEngine.Events;

public class EventOnDisable : MonoBehaviour
{
    [SerializeField] UnityEvent disableEvent = null;

    public void OnDisable()
    {
        if (disableEvent != null)
        {
            disableEvent.Invoke();
        }
    }
}
