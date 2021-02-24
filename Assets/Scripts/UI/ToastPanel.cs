using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastPanel : MonoBehaviour
{
    public bool testActivate = false;
    Coroutine transitionRoutine = null;
    bool active = false;
    bool transitioning = false;
    float timer = 0f;

    [Header("Configure")]
    [SerializeField] Vector2 inactivePosition = new Vector2();
    [SerializeField] Vector2 activePosition = new Vector2();
    [SerializeField] float transitionTime = 1f;
    
    [Header("Reference")]
    [SerializeField] GameObject displayRoot = null;
    [SerializeField] RectTransform rectTransform = null;
    [SerializeField] AudioMod audioMod = null;


    private void OnEnable()
    {
        if(transitioning)
        {
            StartCoroutine(Transition(!active, timer));
        }
    }

    void Update()
    {
        if(testActivate)
        {
            testActivate = false;
            if(transitionRoutine != null)
            {
                StopCoroutine(transitionRoutine);
                transitionRoutine = null;
            }
            transitionRoutine = StartCoroutine(Transition(!displayRoot.activeInHierarchy));
        }
    }

    public void Activate(bool transitionToActive)
    {
        if (transitionRoutine != null)
        {
            StopCoroutine(transitionRoutine);
            transitionRoutine = null;
        }
        transitionRoutine = StartCoroutine(Transition(transitionToActive));
    }

    private IEnumerator Transition(bool transitionToActive, float timerValue = 0f)
    {
        timer = timerValue;
        Vector2 from = new Vector3();
        Vector2 to = new Vector3();

        from = rectTransform.anchoredPosition;
        to = transitionToActive ? activePosition : inactivePosition;

        transitioning = true;
        if (transitionToActive)
        {
            displayRoot.SetActive(true);
        }
        else if(audioMod != null)
        {
            //Debug.Log(audioMod);
            audioMod.PlayAudioClip(0);
        }

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(from, to, timer / transitionTime);
            yield return null;
        }

        if(!transitionToActive)
        {
            displayRoot.SetActive(false);
            active = false;
        }
        else if (audioMod != null)
        {
            //Debug.Log(audioMod);
            audioMod.PlayAudioClip(0);
            active = true;
        }
        transitioning = false;
        yield return null;
    }

    public void SetToastProgress(float percentage)
    {
        rectTransform.anchoredPosition = Vector2.Lerp(inactivePosition, activePosition, percentage);
    }
}
