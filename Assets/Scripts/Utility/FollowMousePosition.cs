using UnityEngine;

public class FollowMousePosition : MonoBehaviour
{
    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        transform.position = pos;
    }
}
