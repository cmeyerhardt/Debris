using UnityEngine;

public class DisplayCase : MonoBehaviour
{
    private void Awake()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
            //destroy THIS or destroy GAMEOBJECT?
            //Destroying THIS will leave an empty gameObject in the scene for all time 
            //Destroying GAMEOBJECT will destroy any other classes on the gameObject too 
        }
    }

    public void DisplayThing(int i)
    {
        Transform child = transform.GetChild(i);
        if(child != null)
        {
            child.gameObject.SetActive(true);
        }
    }
}
