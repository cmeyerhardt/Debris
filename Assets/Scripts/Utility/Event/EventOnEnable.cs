using UnityEngine;
using UnityEngine.Events;

public class EventOnEnable : MonoBehaviour
{
    [SerializeField] UnityEvent enableEvent = null;

    void OnEnable()
    {
        if (enableEvent != null)
        {
            enableEvent.Invoke();
        }
    }
}
