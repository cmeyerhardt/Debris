using TMPro;
using UnityEngine;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText = null;

    private void Awake()
    {
        ScoreTracker s = FindObjectOfType<ScoreTracker>();
        if(s != null)
        {        
            s.updated.AddListener(UpdateInfo);
        }
    }

    public void UpdateInfo(int score)
    {
        if(scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}
