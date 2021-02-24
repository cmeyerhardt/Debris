using UnityEngine;
using UnityEngine.Events;

public class PowerUpEvent : UnityEvent<PowerUp> { }

public class ClickForPoints2D : ClickForPoints
{
    [Header("2D")]
    [SerializeField] CanvasFade fade = null;
    [SerializeField] public RectTransform reposition = null;
    [SerializeField] Animator animator = null;
    [SerializeField] public PowerUp powerUp = PowerUp.None;

    public static PowerUpEvent powerUpEvent = new PowerUpEvent();

    private void Start()
    {
        Invoke("Fade", 5f);
        Destroy(gameObject, 6f);
    }

    public override void SuccessfullyClick()
    {
        if(!pointsEarned)
        {
            pointsEarned = true;
            if (powerUp != PowerUp.None)
            {
                powerUpEvent.Invoke(powerUp);
            }
            //? what should happen if its none? 

            if (successFX != null)
                Instantiate(successFX, Camera.main.ScreenToWorldPoint(reposition.position), Quaternion.identity, null);
            animator.SetTrigger("Clicked");
            Destroy(gameObject, 1f);
        }
    }

    public override void ImpactFloor()
    {
        base.ImpactFloor();
    }

    public override void Freeze(float duration = 5f)
    {
        if (!frozen)
        {
            animator.enabled = false;
            base.Freeze(duration);
        }
    }

    public override void UnFreeze()
    {
        animator.enabled = true;
        base.UnFreeze();
    }

    private void Fade()
    {
        fade.canvasState = CanvasFade.CanvasState.FadeOut;
    }

    public void Reposition(Vector3 position)
    {
        reposition.position = Camera.main.WorldToScreenPoint(position);
    }
}
