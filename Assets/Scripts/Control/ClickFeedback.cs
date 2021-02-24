using UnityEngine;
using UnityEngine.EventSystems;

public class ClickFeedback : MonoBehaviour
{
    //[SerializeField] Texture2D defaultImage = null;
    //[SerializeField] Texture2D clickImage = null;
    //[SerializeField] Texture2D uiImage = null;
    [SerializeField] AudioMod audioMod = null;

    private void Awake()
    {
        //SetCursorImage(defaultImage);
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //SetCursorImage(uiImage);

            // ui click sound ?
            //if (Input.GetMouseButtonDown(0))
            //{
            //  audioMod.PlayAudioClip(1);
            //}
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //SetCursorImage(clickImage);
            if (!ClickedOnSomething())
            {
                if(audioMod != null)
                {
                    audioMod.PlayAudioClip(0);
                }
            }
        }
        //else if (Input.GetMouseButton(0))
        //{
        //    SetCursorImage(clickImage);
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    SetCursorImage(defaultImage);
        //}
        //else
        //{
        //    SetCursorImage(defaultImage);
        //}
    }

    private void SetCursorImage(Texture2D cursor)
    {
        if (cursor != null)
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }
    }

    private bool ClickedOnSomething()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100f, ~(1 << 2));   // ignore layer 2
        if (hits.Length == 0)
        {
            return false;
        }
        return true;
    }
}
