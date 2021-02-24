using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    public enum CanvasState { Idle, FadeIn, FadeOut, Alpha0, Alpha1 }
    [Tooltip("0 - Idle\n1 - FadeIn\n2 - FadeOut\n3 - Alpha0\n4 - Alpha1")]
    [SerializeField] public CanvasState canvasState = CanvasState.Idle;
    [SerializeField] public float fadeTime = 1f;

    CanvasGroup canvasGroup = null;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Update()
    {
        if (canvasGroup == null) { Debug.LogWarning(gameObject.name + "(" + this + ") has no CanvasGroup Component."); return; }

        switch (canvasState)
        {
            case CanvasState.Alpha0:
                canvasGroup.alpha = 0f;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                canvasState = CanvasState.Idle;
                break;
            case CanvasState.Alpha1:
                SetAlpha1();
                break;
            case CanvasState.FadeOut:
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, (1f / fadeTime) * Time.deltaTime);
                if (canvasGroup.alpha == 0f)
                {
                    canvasState = CanvasState.Idle;
                }
                break;
            case CanvasState.FadeIn:
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, (1f / fadeTime) * Time.deltaTime);
                if (canvasGroup.alpha == 1f)
                {
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.interactable = true;
                    canvasState = CanvasState.Idle;
                }
                break;
            case CanvasState.Idle:
            default:
                break;
        }
    }

    private void SetAlpha1()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        canvasState = CanvasState.Idle;
    }

    public void SetAlpha1ThenFade()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        canvasState = CanvasState.FadeOut;
    }

    public void SetState(CanvasState c)
    {
        canvasState = c;
    }

    public void SetState(int i)
    {
        canvasState = (CanvasState)i;
    }
}
