using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceWindow : Window
{
    [Header("Choices")]
    [SerializeField] public TextMeshProUGUI[] choiceText = null;
    public IntEvent choiceEvent = new IntEvent();

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerText = null;
    [SerializeField] Image filledImage = null;
    float t = 10f;

    public void OnEnable()
    {
        t = 10f;
        UpdateTimer();
    }
    
    private void Update()
    {
        if(t > 0f)
        {
            t -= Time.unscaledDeltaTime;
        }
        else
        {
            ChoiceMade(Random.Range(0, 2));
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Choice Forced By Escape");
            ChoiceMade(Random.Range(0, 2));
        }

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if(timerText != null)
        {
            timerText.text = ((int)t).ToString();
        }
        if(filledImage != null)
        {
            filledImage.fillAmount = t / 10f;
        }
    }

    public void MakeRandomChoice()
    {
        ChoiceMade(Random.Range(0, 2));
    }

    public void ChoiceMade(int i)
    {
        choiceEvent.Invoke(i);
    }
}
