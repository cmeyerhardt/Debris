using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text = null;
    float timer = 0f;

    private void Awake()
    {
        timer = 0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if(text != null)
        {
            
            text.text = string.Format("{0:00}:{1:00}", (int)timer / 60, timer % 60);
        }
    }
}
