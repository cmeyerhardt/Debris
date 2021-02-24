using UnityEngine;
using UnityEngine.Events;

public class DelayedAction : MonoBehaviour
{
    public UnityEvent action;
    public float delay = 0f;

    private void Awake()
    {
        if(action != null)
        {
            Invoke("InvokeAction", delay);
            Destroy(this, delay + 1f);
        }
    }

    private void InvokeAction()
    {
        action.Invoke();
    }
}
