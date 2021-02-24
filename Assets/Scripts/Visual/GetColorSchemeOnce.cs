using UnityEngine;

/// <summary>
/// This GetColorSceme child class will only get color once
/// A static RandomColorScheme reference is used to refrain from using FindObjectOfType in multiple instances
/// </summary>
public class GetColorSchemeOnce : GetColorScheme
{
    //public static ColorSchemePicker rcs = null;

    //public override void Start()
    //{
    //    if(rcs == null)
    //    {
    //        rcs = FindObjectOfType<ColorSchemePicker>();
    //    }

    //    if (rcs != null)
    //    {
    //        Recolor(rcs.GetColor(index));
    //    }
    //}

    public override void Recolor(Color newColor)
    {
        base.Recolor(newColor);
        Destroy(this);
    }
}
