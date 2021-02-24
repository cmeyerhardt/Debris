using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashText : MonoBehaviour
{
    [SerializeField] Color color1 = new Color();
    [SerializeField] Color color2 = new Color();
    [SerializeField] TextMeshProUGUI text = null;
    [SerializeField] float interval = 1f;

    void Update()
    {
        text.color = Color.Lerp(color1, color2, Mathf.PingPong(Time.time, interval));
    }

    private void OnDisable()
    {
        text.color = color1;
    }

    private void OnEnable()
    {
        text.color = color1;
    }
}
