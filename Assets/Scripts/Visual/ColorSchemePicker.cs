using UnityEngine;

public class ColorSchemePicker : MonoBehaviour
{

    //public int debugChoose = -1;
    [SerializeField] ColorSet[] colorSets = null;

    void Awake()
    {
        //choose random color scheme
        ColorScheme.scheme = colorSets[Random.Range(0, colorSets.Length)];

        //ColorSet chosenSet = colorSets[Random.Range(0, colorSets.Length)];
        //colorSets = new ColorSet[1] { chosenSet };

        //Debug
        //if (debugChoose < 0)
        //{
        //    ColorSet chosenSet = colorSets[Random.Range(0, colorSets.Length)];
        //    colorSets = new ColorSet[1] { chosenSet };
        //}
        //else
        //{
        //    ColorSet chosenSet = colorSets[debugChoose];
        //    colorSets = new ColorSet[1] { chosenSet };
        //}
    }
}


[System.Serializable]
public class ColorSet
{
    public Color color1 = Color.white;
    public Color color2 = Color.white;
    public Color color3 = Color.white;
    public Gradient gradient = null;
}