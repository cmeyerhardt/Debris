using UnityEngine;
using UnityEngine.Events;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] public UnityEvent pointsLessThanZero;
    public IntEvent updated = new IntEvent();
    public static int score = 30;

    public string cheatCode = "EUREKA";
    public string playerCheatProgress = "";
    float timeSinceLastInput = 0f;

    private void Start()
    {
        score = 30;
        updated.Invoke(score);
    }

    private void Update()
    {
        if(playerCheatProgress.Length > 0)
        {
            timeSinceLastInput -= Time.deltaTime;
            if (timeSinceLastInput > 4f)
            {
                playerCheatProgress = "";
            }
            if(cheatCode == playerCheatProgress)
            {
                playerCheatProgress = "";
                GetCheatPoints();
            }
        }

        // add letters to cheatCode progress string
        if (Input.GetKeyDown(cheatCode[playerCheatProgress.Length].ToString().ToKeyCode()))
        {
            //Debug.Log(cheatCode[playerCheatProgress.Length].ToString() + " was pressed");
            playerCheatProgress += cheatCode[playerCheatProgress.Length].ToString();
        }
    }

    public void GetCheatPoints()
    {
        score = 1000;
        updated.Invoke(score);
    }

    public void ProcessScoreChange(int scoreChange)
    {
        score += scoreChange;
        if(score < 0)
        {
            pointsLessThanZero.Invoke();
        }
        updated.Invoke(score);
    }
}
