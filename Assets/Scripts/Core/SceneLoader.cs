using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //static SceneLoader Instance = null;
    //bool firstTime = true;
    [SerializeField] public CanvasFade loadingScreen = null;
    private void Awake()
    {
        //if (Instance == null && Instance != this)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        //PauseTime();
    }

    public void QuitLevel()
    {
        Application.Quit();
    }

    public void LoadTitleScreen()
    {
        ResumeTime();
        SceneManager.LoadScene(0);
    }

    public void ReloadLevel()
    {
        ResumeTime();
        SceneManager.LoadScene(1);
    }

    private IEnumerator LoadLevelRoutine(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        loadingScreen.SetState(CanvasFade.CanvasState.FadeIn);
        float timer = 0f;
        while(loadingScreen.canvasState != CanvasFade.CanvasState.Idle && timer < 5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForEndOfFrame();

        ReloadLevel();
    }

    public void LoadLevelInSeconds(float time)
    {
        StartCoroutine(LoadLevelRoutine(time));
    }

    public void QuitGameInSeconds(float time)
    {
        Invoke("QuitLevel", time);
    }

    public void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void SlowDownTime()
    {
        StartCoroutine(ProcessTimeScaleChange(0f));
    }

    public void SpeedUpTime()
    {
        StartCoroutine(ProcessTimeScaleChange(1f));
    }

    public IEnumerator ProcessTimeScaleChange(float goal)
    {
        while (Time.timeScale != goal)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, goal, 2f / Time.unscaledDeltaTime);
            yield return null;
        }
    }

    public void ResumeTime()
    {
        Time.timeScale = 1;
    }
}
