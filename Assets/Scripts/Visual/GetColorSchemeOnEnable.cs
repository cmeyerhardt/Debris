
/// <summary>
/// This GetColorSceme child class will get a new color every time the object is enabled
/// A static RandomColorScheme reference is used to refrain from using FindObjectOfType frequently amongst multiple instances
/// </summary>
public class GetColorSchemeOnEnable : GetColorScheme
{
    //static ColorSchemePicker rcs = null;

    //private void Awake()
    //{
    //    if(rcs == null)
    //    {
    //        rcs = FindObjectOfType<RandomColorScheme>();
    //    }
    //}

    //public override void Start()
    //{
    //    Recolor(ColorScheme.GetColor(index));
    //    //if (rcs != null)
    //    //{
    //    //    Recolor(rcs.GetColor(index));
    //    //}
    //}

    private void OnEnable()
    {
        //if (rcs == null)
        //{
        //    rcs = FindObjectOfType<ColorSchemePicker>();
        //}

        //if(rcs != null)
        //{
        //    Recolor(rcs.GetColor(index));
        //}
        Recolor(ColorScheme.GetColor(index));
    }
}
