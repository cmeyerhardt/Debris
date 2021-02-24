using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public enum PowerUp
{
    None,
    FreezeAll,
    DestroyAll,
    MouseOverDestroy //allow player to destroy objects on mouse over
}

public class PowerUpButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onClick;

    [Header("State")]
    public bool isActive = false;
    //public bool costMet = true;
    public bool hasCooldown = false;

    [Header("Configure")]
    [SerializeField] PowerUp powerUp = PowerUp.None;
    [SerializeField] KeyCode activationKey = KeyCode.None;
    [SerializeField] float duration = 0f;
    //[SerializeField] int cost = 10;
    [SerializeField] int numCharges = 2;
    [SerializeField] int NumCharges
    {
        get { return numCharges; }
        set
        {
            numCharges = value;
            SetChargesText(value);

            if (value <= 0 && isActive)
            {
                TurnOnButton(false);
            }
            else if(!isActive)
            {
                TurnOnButton(true);
            }
        }
    }


    public static Dictionary<PowerUp, float> activePowerUps = new Dictionary<PowerUp, float>();
    private static SortedList<PowerUp, Tuple<float, float>> sortedWeights = new SortedList<PowerUp, Tuple<float, float>>();
    [SerializeField] float probabilityMultiplier = 1f;

    [Header("Reference")]
    [SerializeField] GameObject activeFX = null;
    [SerializeField] ToastPanel toast = null;
    [SerializeField] ToastPanel backGroundToast = null;
    //[SerializeField] GameObject displayRoot = null;
    [SerializeField] static ScoreTracker scoreTracker = null;
    [SerializeField] static AudioMod audioMod = null;

    [Header("-- UI")]
    //[SerializeField] TextMeshProUGUI costText = null;
    [SerializeField] TextMeshProUGUI chargesText = null;
    //[SerializeField] Image filledImage = null;
    //[SerializeField] Image cdImage = null;

    public static PowerUp DeterminePowerUp()
    {
        //add all weights and compare to ints, random.range to max int
        float accumulation = 0f;
        if(activePowerUps.Count > 0)
        {
            if(sortedWeights.Count != activePowerUps.Count)
            {
                sortedWeights = new SortedList<PowerUp, Tuple<float, float>>();

                //accumulate weights
                foreach (KeyValuePair<PowerUp, float> powerUp in activePowerUps)
                {
                    sortedWeights.Add(powerUp.Key, new Tuple<float, float>(accumulation, accumulation + powerUp.Value));
                    accumulation += powerUp.Value;
                }
            }
            else
            {
                foreach (KeyValuePair<PowerUp, float> powerUp in activePowerUps)
                {
                    accumulation += powerUp.Value;
                }
            }


            float chosenNumber = UnityEngine.Random.Range(0, accumulation);

            
            foreach (KeyValuePair<PowerUp, Tuple<float, float>> powerUp in sortedWeights)
            {
                if(chosenNumber >= powerUp.Value.Item1 && chosenNumber <= powerUp.Value.Item2)
                {
                    //Debug.Log("Chosen (float) PowerUp: " + powerUp);
                    return powerUp.Key;
                }
            }
        }
        return PowerUp.None;
    }

    //Cache
    //static colleciton of powerup buttoms and powerups


    //public static void AddCharge(PowerUp powerUpToAdd)
    //{
    //    if(powerUp == powerUpToAdd)
    //    {
    //        AddCharge();
    //    }
    //}


    private void Awake()
    {
        ClickForPoints2D.powerUpEvent.AddListener(IncreaseCharge);
        if(scoreTracker == null)
        {
            scoreTracker = FindObjectOfType<ScoreTracker>();
        }

        if(audioMod == null)
        {
            audioMod = GetComponentInParent<AudioMod>();
        }

        if (audioMod == null)
        {
            Debug.Log("AudioMod cannot be found for PowerUpButton");
        }

        //if (displayRoot == null)
        //{
        //    Transform t = transform.Find("DisplayRoot");
        //    if(t != null)
        //    {
        //        displayRoot = t.gameObject;
        //    }
        //}

        //if (displayRoot != null)
        //{
        //    GetDisplayRoot().SetActive(false);
        //}
    }

    private void Start()
    {
        //SetCostText(cost);
        SetChargesText(numCharges);

        onClick.AddListener(() =>
        {
            if (numCharges > 0)
            {
                IEnumerator done = null;
                done = GetButtonAction();

                if (done != null)
                {
                    RemoveCharge();
                    StartCoroutine(done);
                    GoOnCooldown();
                    if (activeFX != null && !activeFX.activeInHierarchy)
                    {
                        activeFX.SetActive(true);
                        StartCoroutine(UsefulStuff.DoAfter(duration, () =>
                        {
                            activeFX.SetActive(false);
                        }));
                    }
                }
            }
            else
            {
                if (audioMod != null)
                {
                    audioMod.PlayAudioClip(0);
                }
            }
        });

        //if (scoreTracker != null)
        //{
            

        //    //scoreTracker.updated.AddListener(SetFillAmount);
        //}
    }


    private IEnumerator GetButtonAction()
    {
        switch (powerUp)
        {
            case PowerUp.MouseOverDestroy:
                if (!FallingObject.passive)
                {
                    FallingObject.passive = true;

                    if(audioMod != null)
                    {
                        audioMod.PlayAudioClip(3);
                    }
                    
                    return UsefulStuff.DoAfter(duration, () =>
                    {
                        FallingObject.passive = false;
                    });
                }
                break;
            case PowerUp.FreezeAll:
                if (!FallingObject.freeze)
                {
                    FallingObject.freeze = true;
                    ObjectSpawner.stopSpawning = true;

                    if (audioMod != null)
                    {
                        audioMod.PlayAudioClip(1);
                    }

                    return UsefulStuff.DoAfter(duration, () =>
                    {
                        FallingObject.freeze = false;
                        ObjectSpawner.stopSpawning = false;
                    });
                }
                break;
            case PowerUp.DestroyAll:
                if (!FallingObject.destroy)  // activate on condition?
                {
                    if (audioMod != null)
                    {
                        audioMod.PlayAudioClip(2);
                    }

                    ObjectSpawner.stopSpawning = true;
                    FallingObject.destroy = true;
                    return UsefulStuff.DoAfter(duration, () =>
                    {
                        FallingObject.destroy = false;
                        ObjectSpawner.stopSpawning = false;
                    });
                }
                break;
            default:
                break;
        }

        return null;
    }

    private void Update()
    {
        if (!isActive || numCharges <=  0 || Time.timeScale < 1f) { return; }
        if (Input.GetKeyDown(activationKey))
        {
            //Debug.Log("Button: " + powerUp + " pressed");
            onClick.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Button: " + powerUp + " clicked");
        onClick.Invoke();
    }
    
    public PowerUp GetPowerUp()
    {
        return powerUp;
    }

    //public void SetCharges(int num)
    //{
    //    numCharges = num;
    //    if (numCharges == 0)
    //    {
    //        TurnOnButton(false);
    //    }
    //    else
    //    {
    //        TurnOnButton(true);
    //    }
    //    SetChargesText(numCharges);
    //}

    public void AddCharge()
    {
        //Debug.Log("Adding Charges: " + name);
        NumCharges++;
        //if(numCharges == 0)
        //{
        //    numCharges++;
        //    SetChargesText(numCharges);

        //    TurnOnButton(true);
        //}
        //else
        //{
        //    numCharges++;
        //    SetChargesText(numCharges);
        //}
    }

    public void RemoveCharge()
    {
        NumCharges--;
        //if(numCharges <= 0) 
        //{
        //    TurnOnButton(false);
        //}
        //else
        //{
        //    SetChargesText(numCharges);
        //}
    }

    // DISPLAY
    //public void SetFillAmount(int numerator)
    //{
    //    if(powerUp == PowerUp.None) { return; }
        
    //    if(filledImage != null)
    //    {
    //        filledImage.fillAmount = Mathf.Clamp(numerator / cost, 0f, 1f);
    //    }

    //    costMet = numerator >= cost;
    //}

    //public void SetCostText(int i)
    //{
    //    if(costText != null)
    //    {
    //        costText.text = i.ToString();
    //    }
    //}
    
    public void SetChargesText(int i)
    {
        if (chargesText != null)
        {
            chargesText.text = i.ToString();
        }
    }

    //public GameObject GetDisplayRoot()
    //{
    //    return displayRoot;
    //}
    
    public void TurnOnButton(bool activate)
    {
        if (toast != null)
        {
            toast.Activate(activate);
        }
        isActive = activate;
        //if (displayRoot != null)
        //{
        //    displayRoot.SetActive(activate);
        //    isActive = activate;
        //}
    }
    
    public void ActivateButton()
    {
        if (!activePowerUps.ContainsKey(powerUp))
        {
            activePowerUps.Add(powerUp, probabilityMultiplier);
        }
        backGroundToast.Activate(true);
        AddCharge();
    }

    public void IncreaseCharge(PowerUp i)
    {
        if (i == powerUp)
        {
            Debug.Log(i);
            AddCharge();
        }
    }



    // COOLDOWN

    public void GoOnCooldown()
    {
        if(isActive)
        {
            isActive = false;
            StartCoroutine(Cooldown(duration));
        }
    }

    public IEnumerator Cooldown(float cooldownTime)
    {
        if(toast != null)
        {
            toast.SetToastProgress(0f);
        }
        //if(cdImage != null)
        //{
        //    cdImage.fillAmount = 1f;
        //}

        float timer = cooldownTime;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            //if (cdImage != null)
            //{
            //    cdImage.fillAmount = timer / cooldownTime;
            //}
            if (toast != null)
            {
                toast.SetToastProgress(timer / cooldownTime);
            }
        }

        if (toast != null)
        {
            toast.SetToastProgress(1f);
        }
        //if (cdImage != null)
        //{
        //    cdImage.fillAmount = 0f;
        //}

        isActive = true;

        yield return null;
    }
}
