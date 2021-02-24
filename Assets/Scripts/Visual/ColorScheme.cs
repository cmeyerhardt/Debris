using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScheme
{
    public static ColorSet scheme;

    public void SetColorScheme(ColorSet _scheme)
    {
        scheme = _scheme;
    }

    public static Color GetColor(int i)
    {
        if (i < 0)
        {
            return GetGradientColor();
        }

        if (scheme != null)
        {
            switch (i)
            {
                case 0:
                    return scheme.color1;
                case 1:
                    return scheme.color2;
                case 2:
                default:
                    return scheme.color3;
            }
        }
        return Color.grey;
    }

    public static Color GetGradientColor(float f = -1)
    {
        if (scheme != null)
        {
            if (scheme.gradient != null)
            {
                return scheme.gradient.Evaluate(f < 0 ? Random.Range(0f, 1f) : f);
            }
        }
        return Color.grey;
    }

    public static Gradient GetGradient()
    {
        if (scheme != null)
        {
            return scheme.gradient;
        }
        return null;
    }
}
