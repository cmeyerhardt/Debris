using UnityEngine;

/// <summary>
/// This GetColorSceme child class will retrieve a color from a RandomColorScheme object in the scene
/// The color is retrieved only once and the class stays active to allow other objects can Recolor() during runtime
/// A static RandomColorScheme reference is used to refrain from using FindObjectOfType 
/// 
/// This class inherits Colorizer, which is sets the color of various object types by way of a switch statement
/// </summary>
public class GetColorScheme : Colorizer
{
    [Header("Index of Color Scheme")]
    [Tooltip("To use gradient, set < 1")][SerializeField] public int index = 0;

    public virtual void Start()
    {
        Recolor(ColorScheme.GetColor(index));
        //RandomColorScheme rcs = FindObjectOfType<RandomColorScheme>();
        //if(rcs != null)
        //{
        //    Recolor(rcs.GetColor(index));
        //}
    }

    public override void Recolor(Color newColor)
    {
        base.Recolor(newColor);
    }
}
