using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//public enum Bonus { AreaFreeze, AreaClick, PassiveClick, ExtraPoints, NewShape }
//public enum ObstacleAugment { AddCube, CubeDisappears, CubeSizeIncrease, CubeSizeDecrease, CubeOscillates, CubeWeapon, CubeMovesUp }
//public enum LevelProgression { IncreaseSpawnRate, IncreaseSpawnWidth }

//todo -- class to house challenges and bonuses, graphics, text, action calls ?
//todo -- criteria for choosing from pool
//todo -- progression/decision trees

public class Progression : MonoBehaviour
{
    [SerializeField] float progressionInterval = 30f;

    [Header("Display")]
    [SerializeField] GameObject pauseBreakText = null;
    [SerializeField] GameObject doingGreatText = null;
    [SerializeField] GameObject doingOkText = null;
    [SerializeField] GameObject doBetterText = null;

    [SerializeField] TextMeshProUGUI successText = null;
    [SerializeField] TextMeshProUGUI missedText = null;
    [SerializeField] TextMeshProUGUI roundText = null;
    [SerializeField] TextMeshProUGUI numRoundsTextLoseMenu = null;
    [SerializeField] TextMeshProUGUI percentSuccessText = null;
    //[SerializeField] GameObject successTrackerGO = null;
    [SerializeField] ToastPanel successToastPanel = null;
    int numRounds = 0;
    int numMissedThisRound = 0;
    int numSuccessThisRound = 0;
    float percentageSuccess = 0f;

    [Header("Scene Management")]
    [SerializeField] SceneLoader sceneLoader = null;
    [SerializeField] WindowManager windowManager = null;
    [SerializeField] ChoiceWindow choiceWindow = null;
    [SerializeField] ScoreTracker scoreTracker = null;

    [Header("Object Management")]
    [SerializeField] ObjectSpawner objectSpawner = null;
    [SerializeField] Transform spawnAreaIndicator = null;
    [SerializeField] ObstacleSpawner obstacleSpawner = null;
    [SerializeField] Spawner badSpawner = null;
    //[SerializeField] AreaClick areaClick = null;

    [Header("Audio")]
    [SerializeField] AudioMod audioMod = null;
    [SerializeField] AudioSource musicPlayer = null;
    [SerializeField] AudioSource fxTrackPlayer = null;

    public enum ActionEnum
    {
        FreezeAll, DestroyAll, MouseOverDestroy, ExtraPoints, NewShape,
        AddCube, CubeDisappears,
        CubeSizeIncrease, CubeSizeDecrease,
        CubeOscillates,
        CubeWeapon,
        CubeMovesUp, CubeMovesDown,
        IncreaseSpawnRate, IncreaseSpawnWidth, DecreaseSpawnRate,
        //MoreBadShapes,
        IncreaseGravity, DecreaseGravity
    }
    
    [Header("Choices")]
    public List<ActionEnum> availableChoices = new List<ActionEnum>();
    public List<ActionEnum> choicesThisTime = new List<ActionEnum>();

    //todo -- use images instead of text
    Dictionary<ActionEnum, string> actionStringDict = new Dictionary<ActionEnum, string>()
    {
                {ActionEnum.AddCube,              "Add an Obstacle"},
                {ActionEnum.CubeDisappears,       "Remove an Obstacle"},
                {ActionEnum.CubeSizeIncrease,     "An Obstacle Grows"},
                {ActionEnum.CubeSizeDecrease,     "An Obstacle Shrinks"},
                {ActionEnum.CubeOscillates,       "An Obstacle Oscillates"},
                {ActionEnum.CubeWeapon,           "Weaponize\nan Obstacle"},
                {ActionEnum.CubeMovesUp,          "An Obstacle Moves Up"},
                {ActionEnum.CubeMovesDown,        "An Obstacle Moves Down"},
                {ActionEnum.FreezeAll,            "Unlock Ability:\nFreeze"},
                {ActionEnum.DestroyAll,           "Unlock Ability:\nMassClick"},
                {ActionEnum.MouseOverDestroy,     "Unlock Ability:\nPassive"},
                {ActionEnum.NewShape,             "New Shape"},
                {ActionEnum.IncreaseSpawnRate,    "Increase Spawn Rate"},
                {ActionEnum.DecreaseSpawnRate,    "Decrease Spawn Rate"},
                {ActionEnum.IncreaseSpawnWidth,   "Increase Spawn Area"},
                //{ActionEnum.MoreBadShapes,        "More Bad Shapes"},
                {ActionEnum.IncreaseGravity,      "Increase Gravity"},
                {ActionEnum.DecreaseGravity,      "Decrease Gravity"}
    };
    Dictionary<ActionEnum, System.Action> actionDictionary;


    // Cache
    float timer = 0f;
    bool choiceBeingMade = false;
    int[] storedPoints = null;
    int choiceMade = 0;

    private void Awake()
    {
        availableChoices = new List<ActionEnum>()
        {
            ActionEnum.FreezeAll
            , ActionEnum.DestroyAll
            , ActionEnum.MouseOverDestroy
            , ActionEnum.ExtraPoints
            , ActionEnum.NewShape
            , ActionEnum.NewShape
            , ActionEnum.NewShape
            , ActionEnum.NewShape
            , ActionEnum.NewShape
            //, ActionEnum.CubeDisappears
            //, ActionEnum.CubeSizeIncrease
            //, ActionEnum.CubeSizeDecrease
            //, ActionEnum.CubeOscillates
            //, ActionEnum.CubeWeapon
            //, ActionEnum.CubeMovesUp
            //, ActionEnum.CubeMovesDown
        };

        //Set Spawn Area Size AND Add Increases To Width to availableChoices
        int startingSizeOfSpawnArea = 6;
        
        if (objectSpawner != null)
        {
            objectSpawner.SetWidth(startingSizeOfSpawnArea);
            objectSpawner.unsuccessful.AddListener(IncreaseMissed);
            objectSpawner.successful.AddListener(IncreaseSuccess);
            int numTimesCanIncreaseWidth = objectSpawner.MaxSpawnBoxWidth - startingSizeOfSpawnArea - 1;

            for (int i = numTimesCanIncreaseWidth; i > 0; i--)
            {
                availableChoices.Add(ActionEnum.IncreaseSpawnWidth);
            }
        }
        if(obstacleSpawner != null)
        {
            obstacleSpawner.SetWidth(startingSizeOfSpawnArea);
        }
        
        Vector3 scale = spawnAreaIndicator.localScale;
        scale.x = startingSizeOfSpawnArea + .5f;
        spawnAreaIndicator.localScale = scale;
        
        actionDictionary = new Dictionary<ActionEnum, System.Action>()
        {
              { ActionEnum.DestroyAll,          DestroyAll }
            , { ActionEnum.FreezeAll,           FreezeAll }
            , { ActionEnum.MouseOverDestroy,    MouseOverDestroy }
            , { ActionEnum.ExtraPoints,         ExtraPoints }
            , { ActionEnum.AddCube,             AddCube }
            , { ActionEnum.NewShape,            NewShape }
            , { ActionEnum.CubeDisappears,      CubeDisappears }
            , { ActionEnum.CubeSizeIncrease,    CubeSizeIncrease }
            , { ActionEnum.CubeSizeDecrease,    CubeSizeDecrease }
            , { ActionEnum.CubeOscillates,      CubeOscillates }
            , { ActionEnum.CubeWeapon,          CubeWeapon }
            , { ActionEnum.CubeMovesUp,         CubeMovesUp }
            , { ActionEnum.CubeMovesDown,       CubeMovesDown }
            , { ActionEnum.IncreaseSpawnRate,   IncreaseSpawnRate}
            , { ActionEnum.DecreaseSpawnRate,   DecreaseSpawnRate}
            , { ActionEnum.IncreaseSpawnWidth,  IncreaseSpawnWidth}
            //, { ActionEnum.MoreBadShapes,       MoreBadShapes}
            , { ActionEnum.IncreaseGravity,     IncreaseGravity}
            , { ActionEnum.DecreaseGravity,     DecreaseGravity}
        };

        ReRandomSeed();
        SetSpawnerPause(true);
    }

    public void SetSpawnerPause(bool b)
    {
        Spawner.pause = b;
    }

    private void Start()
    {
        StartCoroutine(InitializeRoutine());
    }

    private IEnumerator InitializeRoutine()
    {
        sceneLoader.loadingScreen.SetState(CanvasFade.CanvasState.FadeOut);
        float timer = 0f;
        while (sceneLoader.loadingScreen.canvasState != CanvasFade.CanvasState.Idle && timer < 5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        sceneLoader.PauseTime();
        windowManager.ShowAwakeMenu();
        musicPlayer.gameObject.SetActive(true);
        ResetSuccessMissedTracking(false);
    }

    public void BeginMessage()
    {
        ShowRoundText();
    }

    private static void ReRandomSeed()
    {
        Random.InitState(Random.Range(0, 255));
    }

    void Update()
    {
        if(Time.timeScale < 1f) { return; }

        timer += Time.deltaTime;
        if(timer > progressionInterval)
        {
            StartCoroutine(PrepareForChoiceSelection());
            timer = 0f;
        }
    }

    //public void MoreBadShapes()
    //{
    //    badSpawner.IncreaseSpawnRate();
    //}

    public void IncreaseMissed()
    {
        numMissedThisRound++;
        missedText.text = numMissedThisRound.ToString();
        RecalculateAndDisplaySuccess();
    }

    public void IncreaseSuccess()
    {
        numSuccessThisRound++;
        successText.text = numSuccessThisRound.ToString();
        RecalculateAndDisplaySuccess();
    }

    private void RecalculateAndDisplaySuccess()
    {
        percentageSuccess = 100f * (numSuccessThisRound / (float)(numSuccessThisRound + numMissedThisRound));
        percentSuccessText.text = string.Format("{0:0}", percentageSuccess);
    }

    public void ResetSuccessMissedTracking(bool activate = true)
    {
        numSuccessThisRound = 0;
        numMissedThisRound = 0;
        missedText.text = numMissedThisRound.ToString();
        successText.text = numSuccessThisRound.ToString();
        percentageSuccess = 0f;
        percentSuccessText.text = string.Format("{0:0}", percentageSuccess);
        if(activate)
            successToastPanel.Activate(true);
    }

    public IEnumerator PrepareForChoiceSelection()
    {
        //Initiate Pause in Spawning
        Spawner.pause = true;
        //obstacleSpawner.pause = true;
        //objectSpawner.pause = true;
        //badSpawner.pause = true;
        pauseBreakText.SetActive(true);

        DontClickMe.Fly = true;
        ObstacleDestroyer.Fly = true;

        //int numBadShapes = badSpawner.transform.childCount;
        //float timer = 0f;
        //while(numBadShapes > 0 && timer < 5f)
        //{ 
        //    DontClickMe d = badSpawner.GetComponentInChildren<DontClickMe>();
        //    if (d != null)
        //    {
        //        d.Destruct();
        //    }
        //    timer += Time.deltaTime;
        //    yield return null;
        //    numBadShapes = badSpawner.transform.childCount;

        //}


        timer = 0f;
        while(objectSpawner.transform.childCount > 0 && timer < 3f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        pauseBreakText.SetActive(false);
        successToastPanel.Activate(false);

        //Reward player's last round success with obstacles moving down (proportionate to percentage success)
        float total = numSuccessThisRound + numMissedThisRound;
        int percentageSuccess = Mathf.RoundToInt(numSuccessThisRound / total * 100f);

        GameObject resultText = null;
        if (percentageSuccess >= 75)
        {
            resultText = doingGreatText;
        }
        else if(percentageSuccess >= 25)
        {
            resultText = doingOkText;
        }
        else
        {
            resultText = doBetterText;
        }
        resultText.SetActive(true);
        yield return new WaitForSeconds(.5f);

        List<Obstacle> obstacles = obstacleSpawner.GetComponentsInChildren<Obstacle>(true).ConvertToList().RemoveNullReferences();
        int numTotalObstacles = Mathf.Max(0, Mathf.Min(obstacles.Count, 6));
        int numObstaclesGoodEffect = Mathf.Min(Mathf.RoundToInt(percentageSuccess / 100f * numTotalObstacles), numTotalObstacles);

        float interval = Mathf.Min(.6f, 3f / numTotalObstacles);
        for (int i = 0; i < numTotalObstacles; i++)
        {
            Obstacle o = obstacles[i];
            if (o != null)
            {
                if(i < numObstaclesGoodEffect)
                {

                    bool disabled = !o.gameObject.activeInHierarchy;
                    if (disabled) { o.gameObject.SetActive(true); }

                    if (Random.Range(0, 20) > 0)
                    {
                        //Debug.Log("Moving Down: " + i);
                        ObstacleMovesDown(o);
                    }
                    else
                    {
                        //Debug.Log("Decreasing Size: " + i);
                        ObstacleDecreasesInSize(o);
                    }

                    if (disabled) { o.gameObject.SetActive(false); }
                    yield return new WaitForSeconds(interval);
                }
                else
                {
                    if (obstacleSpawner.transform.childCount < 30 && Random.Range(0, 20) > 0)
                    {
                        //Debug.Log("Spawning: " + i);
                        _ = obstacleSpawner.Spawn();
                    }
                    else
                    {
                        //Debug.Log("Increasing Size: " + i);
                        ObstacleIncreasesInSize(o);
                    }

                    yield return new WaitForSeconds(interval);
                }
            }
        }

        resultText.SetActive(false);

        DontClickMe.Fly = false;
        ObstacleDestroyer.Fly = false;

        yield return new WaitForSeconds(1f);

        if (CreateChoice())
        {
            if (sceneLoader != null)
            {
                yield return StartCoroutine(sceneLoader.ProcessTimeScaleChange(0f));
            }
            else
            {
                Debug.LogWarning("Progression does not have a sceneLoader reference!");
            }

            choiceBeingMade = true;
            if (choiceWindow != null)
            {
                choiceWindow.choiceEvent.AddListener(ChoiceMade);
                windowManager.OpenWindow(choiceWindow);
            }
        }
    }



    bool CreateChoice()
    {
        ReRandomSeed();
        choicesThisTime.Clear();
        storedPoints = new int[2];

        // Get Two Actions from which Player Chooses
        List<ActionEnum> availableChoicesThisTime = CompileListOfPossibleActions();

        if (availableChoicesThisTime.Count == 0)
        {
            return false;
        }
        else if (availableChoicesThisTime.Count == 1)
        {
            choicesThisTime.Add(availableChoicesThisTime[0]);
            choicesThisTime.Add(availableChoicesThisTime[0]);
            //return;
        }
        else if (availableChoicesThisTime.Count == 2)
        {
            choicesThisTime.Add(availableChoicesThisTime[0]);
            choicesThisTime.Add(availableChoicesThisTime[1]);
            //return;
        }
        int c = 0;
        while (choicesThisTime.Count < 2 && c < 10)
        {
            ActionEnum newChoice = availableChoicesThisTime[UnityEngine.Random.Range(0, availableChoicesThisTime.Count)];

            if (choicesThisTime.Contains(newChoice))
            {
                c++;
            }
            else
            {
                choicesThisTime.Add(newChoice);
            }
        }

        //Debug.Log("Choice Attempts: " + c);

        if (choicesThisTime.Count < 2)
        {
            //Debug.Log("Choice UNSUCCESSFUL!");
            while (choicesThisTime.Count < 2)
            {
                choicesThisTime.Add((ActionEnum)choicesThisTime.Count);
            }
        }

        if(choicesThisTime.Count == 2)
        {
            //Debug.Log("Choices available this time: " + choicesThisTime[0] + " and " + choicesThisTime[1]);

            for (int i = 0; i < choicesThisTime.Count; i++)
            {
                if(choicesThisTime[i] == ActionEnum.ExtraPoints)
                {
                    storedPoints[i] = UnityEngine.Random.Range(5, 15);
                    if(choiceWindow != null)
                    {
                        if(choiceWindow.choiceText[i] != null)
                        {
                            choiceWindow.choiceText[i].text = "Extra Points:\n" + storedPoints[i].ToString();
                        }
                    }
                }
                else
                {
                    if (choiceWindow != null)
                    {
                        if (choiceWindow.choiceText[i] != null)
                        {
                            choiceWindow.choiceText[i].text = actionStringDict[choicesThisTime[i]];
                        }
                    }
                }
            }
        }
        return true;
    }

    public List<ActionEnum> CompileListOfPossibleActions(bool includeDynamicChoices = true)
    {
        List<ActionEnum> localList = new List<ActionEnum>();
        localList.AddRange(availableChoices);
        /*localList already contains: (if unconsumed)
            ActionEnum.AreaFreeze
            ActionEnum.AreaClick
            ActionEnum.PassiveClick
            ActionEnum.ExtraPoints
            ActionEnum.NewShape (multiple)
            ActionEnum.IncreaseSpawnWidth (multiple)
         */

        //Debug.Log("ObstacleSpawner child count: " + obstacleSpawner.transform.childCount + " GCIC: " + obstacleSpawner.GetComponentsInChildren<Obstacle>().Length);
        if (includeDynamicChoices)
        {
            if (numRounds > 1)
            {
                if (numRounds % 3 == 0) //more good choices
                {
                    availableChoices.Add(ActionEnum.DecreaseGravity);

                    if (UnityEngine.Random.Range(0, 2) == 0 && musicPlayer.pitch >= .915)
                    {
                        localList.Add(ActionEnum.DecreaseSpawnRate);
                    }
                }
                else // more bad choices
                {
                    availableChoices.Add(ActionEnum.IncreaseGravity);

                    if (UnityEngine.Random.Range(0, 2) == 0 && musicPlayer.pitch <= 1.3f)
                    {
                        localList.Add(ActionEnum.IncreaseSpawnRate);
                    }
                }
            }


            //if (Time.time > 60f)
            //{
            //    if (UnityEngine.Random.Range(0, 2) == 0)
            //    {
            //        localList.Add(ActionEnum.IncreaseSpawnWidth);
            //    }
            //    if (UnityEngine.Random.Range(0, 2) == 0)
            //    {
            //        localList.Add(ActionEnum.IncreaseSpawnRate);
            //    }
            //    //if (UnityEngine.Random.Range(0, 2) == 0)
            //    //{
            //    //    availableChoices.Add(ActionEnum.MoreBadShapes);
            //    //}
            //    if (UnityEngine.Random.Range(0, 2) == 0)
            //    {
            //        availableChoices.Add(ActionEnum.IncreaseGravity);
            //    }
            //    else
            //    {
            //        availableChoices.Add(ActionEnum.DecreaseGravity);
            //    }
            //}

            int numObstacles = 0;
            if (obstacleSpawner != null)
            {
                numObstacles = obstacleSpawner.transform.childCount;
            }

            if (numObstacles > 0)
            {
                if (numRounds % 3 == 0)
                {
                    localList.Add(ActionEnum.CubeMovesDown);
                }
                else
                {
                    localList.Add(ActionEnum.CubeMovesUp);
                }

                if (numObstacles > 1 && numObstacles < 6)
                {
                    if (numRounds % 3 == 0)
                    {
                        localList.Add(ActionEnum.CubeSizeDecrease);
                    }
                    else
                    {
                        localList.Add(ActionEnum.CubeSizeIncrease);
                    }
                }

                if(numObstacles > 10)
                {
                    localList.Add(ActionEnum.CubeSizeDecrease);
                }

                if (numObstacles > 2)
                {
                    //todo: should a "cube" exist that is not already oscillating?
                    //if(numOscillators/numObstacles > percentThreshold)
                    if (numRounds % 3 == 0)
                    {
                        localList.Add(ActionEnum.CubeWeapon);
                    }
                    else
                    {
                        localList.Add(ActionEnum.CubeOscillates);
                    }
                    //localList.Add(ActionEnum.CubeOscillates);
                    //localList.Add(ActionEnum.CubeWeapon);
                }
            } 

            if (numObstacles > 3)
            {
                if (numRounds % 3 == 0)
                {
                    localList.Add(ActionEnum.CubeWeapon);
                }
                else
                {
                    localList.Add(ActionEnum.CubeDisappears);
                }
                //localList.Add(ActionEnum.CubeDisappears);

                //for (int i = 0; i < numObstacles - 5; i++)
                //{
                //    localList.Add(ActionEnum.CubeDisappears);
                //}
            } 
        }
        return localList;
    }

    public void ChoiceMade(int i)
    {
        if(choiceBeingMade)
        {
            choiceMade = i;
            choiceBeingMade = false;

            StartCoroutine(DoAfter(i));
            //Debug.Log(choicesThisTime[i]);
            //actionDictionary[choicesThisTime[i]]();
            switch (choicesThisTime[i])
            {
                case ActionEnum.DestroyAll:
                case ActionEnum.FreezeAll:
                case ActionEnum.MouseOverDestroy:
                case ActionEnum.NewShape:
                case ActionEnum.IncreaseSpawnWidth:
                //case ActionEnum.MoreBadShapes:
                    //Debug.Log(choicesThisTime[i] + " to be removed from AvailableChoices");
                    availableChoices.Remove(choicesThisTime[i]);
                    break;
            }
            //Debug.Log(choicesThisTime.PrintCollection(" "));

            if (windowManager != null && choiceWindow != null)
            {
                windowManager.CloseWindow(choiceWindow, true);
            }
            else
            {
                Debug.LogWarningFormat("(Progression) NullReference: WindowManager: {0}, ChoiceWindow: {1}", windowManager, choiceWindow);
            }

            ShowRoundText();
        }
    }

    private void ShowRoundText()
    {
        numRounds++;
        roundText.text = "Round " + numRounds + " Begins";
        if(numRounds == 1)
        {
            numRoundsTextLoseMenu.text = "You made it " + numRounds + " round!";
        }
        else
        {
            numRoundsTextLoseMenu.text = "You made it " + numRounds + " rounds!";
        }

        roundText.gameObject.SetActive(true);
        StartCoroutine(UsefulStuff.DoAfter(1.5f, () => { roundText.gameObject.SetActive(false); }));
    }

    private IEnumerator DoAfter(int i)
    {
        yield return new WaitForSeconds(.5f);
        if (actionDictionary.ContainsKey(choicesThisTime[i]))
        {
            actionDictionary[choicesThisTime[i]]();
            ResetSuccessMissedTracking();
            Spawner.pause = false;
            //obstacleSpawner.pause = false;
            //objectSpawner.pause = false;
        }
        else
        {
            Debug.LogWarning("DoAfter coroutine could not call action because it was not defined in action dictionary: " + choicesThisTime[i].ToString());
        }
    }

    public void NewShape()
    {
        if(objectSpawner != null)
        {
            objectSpawner.IncreaseNumberOfShapes();
        }
    }

    public void IncreaseGravity()
    {
        Physics.gravity -= Vector3.up * .5f;
    }

    public void DecreaseGravity()
    {
        Physics.gravity += Vector3.up * .5f;
    }


    public void IncreaseSpawnRate()
    {
        if (objectSpawner != null)
            objectSpawner.IncreaseSpawnRate();

        if (musicPlayer != null && fxTrackPlayer != null)
        {
            musicPlayer.pitch = musicPlayer.pitch * 1.02f;
            fxTrackPlayer.pitch = fxTrackPlayer.pitch * 1.02f;
        }

    }


    public void DecreaseSpawnRate()
    {
        if (objectSpawner != null)
            objectSpawner.IncreaseSpawnRate();

        if (musicPlayer != null)
        {
            musicPlayer.pitch = musicPlayer.pitch * .98f;
            fxTrackPlayer.pitch = fxTrackPlayer.pitch * .98f;
        }
    }


    public void IncreaseSpawnWidth()
    {
        if (objectSpawner != null)
            objectSpawner.IncreaseSpawnWidth();

        if (obstacleSpawner != null)
        {
            obstacleSpawner.IncreaseSpawnWidth();
        }
        if (spawnAreaIndicator != null)
        {
            Vector3 scale = spawnAreaIndicator.localScale;
            scale.x = obstacleSpawner.spawnBoxSize.x + .5f;
            spawnAreaIndicator.localScale = scale;
        }
    }

    public void DestroyAll()
    {
        if(audioMod != null)
        {
            audioMod.PlayAudioClip(0);
        }

        foreach(PowerUpButton b in FindObjectsOfType<PowerUpButton>())
        {
            if(b != null && b.GetPowerUp() == PowerUp.DestroyAll)
            {
                b.ActivateButton();
                //b.AddCharge();
                //b.TurnOnButton(true);
            }
        }
    }

    public void FreezeAll()
    {
        if (audioMod != null)
        {
            audioMod.PlayAudioClip(0);
        }

        foreach (PowerUpButton b in FindObjectsOfType<PowerUpButton>())
        {
            if (b != null && b.GetPowerUp() == PowerUp.FreezeAll) 
            {
                b.ActivateButton();
                //b.AddCharge();
                //b.TurnOnButton(true);
            }
        }
    }

    public void MouseOverDestroy()
    {
        if (audioMod != null)
        {
            audioMod.PlayAudioClip(0);
        }

        foreach (PowerUpButton b in FindObjectsOfType<PowerUpButton>())
        {
            if (b != null && b.GetPowerUp() == PowerUp.MouseOverDestroy) 
            {
                b.ActivateButton();
                //b.AddCharge();
                //b.TurnOnButton(true);
            }
        }
    }

    public void ExtraPoints()
    {
        audioMod.PlayAudioClip(2);

        //todo -- static method call?
        if(scoreTracker != null)
        {
            scoreTracker.ProcessScoreChange(storedPoints[choiceMade]);
        }
        else
        {
            FindObjectOfType<ScoreTracker>().ProcessScoreChange(storedPoints[choiceMade]);
        }

    }

    public void AddCube()
    {
        if(obstacleSpawner != null)
        {
            _ = obstacleSpawner.Spawn();
        }
    }






    //private Obstacle[] GetObstacles()
    //{
    //    if(obstacleSpawner != null)
    //    {
    //        return obstacleSpawner.GetComponentsInChildren<Obstacle>();
    //    }
    //    return null;
    //}

    //Modifications to existing obstacles
    private Obstacle GetRandomObstacle(out bool wasDisabled)
    {
        Obstacle o = null;
        wasDisabled = false;
        if (obstacleSpawner != null)
        {
            int i = UnityEngine.Random.Range(0, obstacleSpawner.transform.childCount);

            Transform t = obstacleSpawner.transform.GetChild(i);
            if (t == null) { return o; }

            o = t.GetComponent<Obstacle>();
            if (o != null && !o.gameObject.activeInHierarchy)
            {
                o.gameObject.SetActive(true);
                wasDisabled = true;
            }
        }
        return o;
    }

    private Obstacle GetRandomActiveObstacle(Obstacle notEqual = null)
    {
        Obstacle o = null;
        if (obstacleSpawner != null)
        {
            List<Obstacle> obstacles = obstacleSpawner.GetComponentsInChildren<Obstacle>().ConvertToList();
            obstacles.RemoveNullReferences();

            if(obstacles.Contains(notEqual))
            {
                obstacles.Remove(notEqual);
            }

            List<Obstacle> toRemove = new List<Obstacle>();
            int oc = obstacles.Count;
            for(int h = 0; h < oc; h++)
            {
                if (!obstacles[h].gameObject.activeInHierarchy)
                {
                    toRemove.Add(obstacles[h]);
                }
            }

            foreach(Obstacle ob in toRemove)
            {
                if (obstacles.Contains(ob))
                {
                    obstacles.Remove(ob);
                }
            }

            return obstacles[Random.Range(0, obstacles.Count)];
        }
        return o;
    }

    private Obstacle GetHighestObstacle()
    {
        Obstacle o = null;
        if (obstacleSpawner != null)
        {
            int i = UnityEngine.Random.Range(0, obstacleSpawner.transform.childCount);

            Transform t = obstacleSpawner.transform.GetChild(i);
            if (t == null) { return o; }

            //foreach
            //sorted list by height

            o = t.GetComponent<Obstacle>();
            if (o != null && !o.gameObject.activeInHierarchy)
            {
                o.gameObject.SetActive(true);
            }
        }
        return o;
    }




    public void CubeSizeIncrease()
    {
        Obstacle o = GetRandomObstacle(out bool d);
        if (o != null)
        {
            ObstacleIncreasesInSize(o);
        }
        if (d)
        {
            o.gameObject.SetActive(false);
        }
    }

    public void CubeSizeDecrease()
    {
        Obstacle o = GetRandomObstacle(out bool d);
        if (o != null)
        {
            ObstacleDecreasesInSize(o);
        }
        if (d)
        {
            o.gameObject.SetActive(false);
        }
    }


    public void ObstacleDecreasesInSize(Obstacle o)
    {
        if (o != null)
        {
            o.DecreaseSize();
        }
    }

    public void ObstacleIncreasesInSize(Obstacle o)
    {
        if (o != null)
        {
            o.IncreaseSize();
        }
    }

    public void CubeMovesUp()
    {
        Obstacle o = GetRandomObstacle(out bool disabled);

        if (o == null) { return; }

        if (obstacleSpawner != null)
        {
            if(o.MoveUp())
            {
                if (o.transform.position.y > (obstacleSpawner.spawnBoxCenter.y + obstacleSpawner.spawnBoxSize.y))
                {
                    Destroy(o.gameObject);
                }
            }

            if (disabled)
            {
                o.gameObject.SetActive(false);
            }
        }
    }

    public void ObstacleMovesDown(Obstacle o)
    {
        if(o == null) { return; }

        bool disable = false;

        if (!o.gameObject.activeInHierarchy)
        {
            disable = true;
            o.gameObject.SetActive(true);
        }

        if (obstacleSpawner != null)
        {
            if(o.MoveDown())
            {
                //Debug.Log("Threshold: " + (obstacleSpawner.spawnBoxCenter.y - obstacleSpawner.spawnBoxSize.y));
                //if (o.transform.position.y <= /*.5f*/(obstacleSpawner.spawnBoxCenter.y - obstacleSpawner.spawnBoxSize.y))
                //{
                //    //Debug.Log(o.transform.position);
                //    o.Destruct();
                //    //Destroy(o.gameObject);
                //}
            }

            if (disable)
            {
                o.gameObject.SetActive(false);
            }
        }
    }


    public void CubeMovesDown()
    {
        //todo -- optimize
        Obstacle o = GetRandomObstacle(out bool disabled);
        ObstacleMovesDown(o);
        //if (o == null) { return; }

        //if (obstacleSpawner != null)
        //{
        //    o.MoveDown();

        //    if (o.transform.position.y < (obstacleSpawner.spawnBoxCenter.y - obstacleSpawner.spawnBoxSize.y))
        //    {
        //        Destroy(o.gameObject);
        //    }
        //    else
        //    {
        //        Oscillator c = o.GetComponent<Oscillator>();
        //        if (c != null)
        //            c.ShiftStartingPositionDown();
        //    }

        //    if (disabled)
        //    {
        //        o.gameObject.SetActive(false);
        //    }
        //}
    }

    public void CubeOscillates()
    {
        List<Oscillator> oscillators = obstacleSpawner.GetComponentsInChildren<Oscillator>(true).ConvertToList().RemoveNullReferences();

        Obstacle o = null;
        int attempts = 0;
        bool d = false;

        while (o == null || !oscillators.Contains(o.GetComponent<Oscillator>()) && attempts < 5)
        {
            o = GetRandomObstacle(out d);
            attempts++;
        }
        if (o != null)
        {
            Oscillator c = o.GetOscillator();
            
            if(c != null)
            {
                if (1.PlusOrMinus() >= 0)
                {
                    c.IncreaseSpeed();
                }
                else
                {
                    c.IncreaseMagnitude();
                }
            }
            else
            {
                o.SetOscillator(o.gameObject.AddComponent<Oscillator>());
            }

            o.PlaySound();

            if (d)
            {
                o.gameObject.SetActive(false);
            }
        }
    }

    public void CubeDisappears()
    {
        Obstacle o = GetRandomObstacle(out bool d);
        if(o != null)
        {
            o.Destruct();
            //Destroy(o.gameObject);
        }
    }

    public void CubeWeapon()
    {
        Obstacle o = GetRandomObstacle(out bool d);

        if(o != null)
        {
            if(o.GetComponent<DestroyOther>() == null)
            {
                DestroyOther DO = o.gameObject.AddComponent<DestroyOther>();
                Obstacle other = GetRandomActiveObstacle(o);
                DO.SetTarget(other, o);
            }
            //if(o.GetAreaClick() != null)
            //{
            //    o.GetAreaClick().ModifyRadius(1.5f);
            //}
            //else if(areaClick != null)
            //{
            //    Instantiate(areaClick, o.transform);
            //}
            //if (d)
            //{
            //    o.gameObject.SetActive(false);
            //}
        }
    }
}
