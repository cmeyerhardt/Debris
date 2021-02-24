using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheatCode : MonoBehaviour
{
    [SerializeField] UnityEvent cheatCodeSuccessful = new UnityEvent();
    public string cheatCode = "EUREKA";
    public string playerCheatProgress = "";
    float timeSinceLastInput = 0f;

    private void Update()
    {
        if (playerCheatProgress.Length > 0)
        {
            timeSinceLastInput -= Time.deltaTime;
            if (timeSinceLastInput > 4f)
            {
                playerCheatProgress = "";
            }
            if (cheatCode == playerCheatProgress)
            {
                playerCheatProgress = "";
                cheatCodeSuccessful.Invoke();
            }
        }

        // add letters to cheatCode progress string
        if (Input.GetKeyDown(cheatCode[playerCheatProgress.Length].ToString().ToKeyCode()))
        {
            //Debug.Log(cheatCode[playerCheatProgress.Length].ToString() + " was pressed");
            playerCheatProgress += cheatCode[playerCheatProgress.Length].ToString();
        }
    }
}
