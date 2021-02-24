using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [Header("Open Windows")]
    [SerializeField] List<Window> openWindows = new List<Window>();

    [Header("Windows")]
    [SerializeField] Window escapeWindow = null;
    [SerializeField] Window tutorialWindow = null;
    [SerializeField] Window loseWindow = null;
    [SerializeField] Window titleWindow = null;

    [Header("Windows Configure")]
    [SerializeField] GameObject menuToBeHidden = null;
    [SerializeField] Window menuToShowOnAwake = null;

    [Header("Reference")]
    [SerializeField] Transform windowsTransform = null;

    private void Awake()
    {
        if (Window.windowManager == null)
        {
            Window.windowManager = this;
        }
        else
        {
            Debug.LogWarning("Duplicate WindowManager found on " + gameObject.name);
            Destroy(this);
        }

        foreach (Window window in windowsTransform.GetComponentsInChildren<Window>())
        {
            CloseWindow(window, false);
            window.gameObject.SetActive(false);
        }
    }

    public void ShowAwakeMenu()
    {
        if (menuToShowOnAwake != null)
        {
            OpenWindow(menuToShowOnAwake);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openWindows.Count > 0)
            {
                CloseWindow(openWindows[openWindows.Count - 1], true);
            }
            else
            {
                OpenWindow(escapeWindow);
            }
        }
    }

    public void OpenEscapeWindow()
    {
        OpenWindow(escapeWindow);
    }

    public void OpenTutorialWindow()
    {
        //Debug.Log("OpenTutorialWIndow");
        //SaveWindow(escapeWindow);

        CloseWindow(escapeWindow, false);
        OpenWindow(tutorialWindow);
    }

    public void CloseTutorialWindow()
    {
        //Debug.Log("OpenTutorialWIndow");
        //SaveWindow(escapeWindow);

        CloseWindow(tutorialWindow, true);
        if (escapeWindow != null)
        {
            OpenWindowNoEvent(escapeWindow);
        }
        else
        {
            menuToBeHidden.SetActive(true);
        }
    }

    public void OpenLoseWindow()
    {
        OpenWindow(loseWindow);
    }

    public void OpenTitleWindow()
    {
        OpenWindow(titleWindow);
    }

    //public void CloseWindowWithEvent(Window windowToClose)
    //{
    //    Debug.Log("CloseWindow:" + windowToClose);
    //    CloseWindow(windowToClose, true);
    //}

    //public void CloseWindowAndRecover(Window windowToClose)
    //{
    //    Debug.Log("Recover and close");
    //    CloseWindow(windowToClose, true, true);
    //}

    //public void CloseWindowNoEventWithRecover(Window windowToClose)
    //{
    //    Debug.Log("CloseWindow:" + windowToClose);
    //    CloseWindow(windowToClose, false, true);
    //}
    public void CloseWindowNoEvent(Window windowToClose)
    {
        CloseWindow(windowToClose, false);
    }

    public void CloseWindow(Window windowToClose)
    {
        CloseWindow(windowToClose, true);
    }

    public void CloseWindow(Window windowToClose, bool doEvent/*, bool recover = false*/)
    {
        Debug.Log("CloseWindow:" + windowToClose);
        if (windowToClose == null/* || (!openWindows.Contains(windowToClose) && !windowsToReEnable.Contains(windowToClose))*/)
                { return; }
        if(doEvent)
            windowToClose.closeEvent.Invoke();


        openWindows.Remove(windowToClose);
        windowToClose.gameObject.SetActive(false);

        //if(recover)
        //{
        //    Window w = RecoverWindow();
        //    if(w != null)
        //    {
        //        OpenWindow(RecoverWindow());
        //    }
        //}

        if (openWindows.Count == 0 && menuToBeHidden != null)
        {
            menuToBeHidden.SetActive(true);
        }
    }

    //public void SaveEscapeWindow()
    //{
    //    SaveWindow(escapeWindow);
    //}

    //public void SaveWindow(Window windowToSave)
    //{
    //    //Debug.Log("SaveWindow:" + windowToSave);
    //    if (windowToSave != null)
    //    {
    //        windowsToReEnable.Push(windowToSave);
    //        openWindows.Remove(windowToSave);
    //        windowToSave.gameObject.SetActive(false);
    //        //CloseWindow(windowToSave, false);
    //    }
    //}

    //public Window RecoverWindow()
    //{
    //    if (windowsToReEnable.Count > 0)
    //    {
    //        //Debug.Log("Recovering");
    //        return windowsToReEnable.Pop();
    //    }
    //    return null;
    //}

    public void OpenWindowWithEvent(Window windowToOpen)
    {
        OpenWindow(windowToOpen);
    }

    public void OpenWindowNoEvent(Window windowToOpen)
    {
        if (windowToOpen == null) { return; }
        //do not reopen window if already open
        if (openWindows.Contains(windowToOpen)) { return; }

        //if (openWindows.Count > 0 && oneWindowAtOnce)
        //{
        //    Window window = openWindows[0];
        //    if (window != null)
        //    {
        //        SaveWindow(window);
        //    }
        //}
        openWindows.Add(windowToOpen);

        windowToOpen.gameObject.SetActive(true);
        if (windowToOpen.hideMenu && menuToBeHidden != null)
        {
            menuToBeHidden.SetActive(false);
        }
    }

    public void OpenWindow(Window windowToOpen)
    {
        if(windowToOpen == null) { return; }
        //do not reopen window if already open
        if (openWindows.Contains(windowToOpen)) { return; }

        //if (openWindows.Count > 0 && oneWindowAtOnce)
        //{
        //    Window window = openWindows[0];
        //    if (window != null)
        //    {
        //        SaveWindow(window);
        //    }
        //}
        openWindows.Add(windowToOpen);

        windowToOpen.gameObject.SetActive(true);
        if (windowToOpen.hideMenu && menuToBeHidden != null)
        {
            menuToBeHidden.SetActive(false);
        }

        windowToOpen.openEvent.Invoke();

    }

    public void CloseWindows(bool doEvent = true)
    {
        foreach (Window openWindow in openWindows)
        {
            if(doEvent)
                openWindow.closeEvent.Invoke();
            openWindow.gameObject.SetActive(false);
        }
        openWindows.Clear();
        menuToBeHidden.SetActive(true);
    }

}
