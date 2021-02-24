using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class AudioUtilities
{
    public static GameObject PlayClipAtPoint(Vector3 position, AudioClip clip, float pitch, float maxDistance, float volume)
    {
        GameObject _temp = new GameObject("TempAudio");
        _temp.transform.position = position;

        AudioSource _source = _temp.AddComponent<AudioSource>();
        _source.clip = clip;
        _source.pitch = pitch;
        _source.maxDistance = maxDistance;
        _source.volume = volume;

        _source.Play();

        GameObject.Destroy(_temp, clip.length);
        return _temp;
    }
}

public static class CollectionExtensions
{

    public static void RandomizeContents<T>(this List<T> list)
    {
        List<T> newList = new List<T>();
        int n = 0;
        int o = list.Count;
        while (n < o)
        {
            newList.Add(list.RemoveFrom());
            n = newList.Count;
            o = list.Count;
        }
    }

    public static bool ContainsReversePair<T>(this List<System.Tuple<T, T>> collection, System.Tuple<T, T> toCheck)
    {
        foreach (System.Tuple<T, T> pairing in collection)
        {
            if (ReferenceEquals(pairing.Item1, toCheck.Item2) && ReferenceEquals(pairing.Item2, toCheck.Item1))
            {
                return true;
            }
        }
        return false;
    }


    ///// <summary>
    ///// Removes a random element from a collection and returns the removed element
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="collection"></param>
    ///// <returns></returns>
    //public static T RemoveFrom<T>(this T[] collection) //todo - test using array
    //{
    //    int index = Random.Range(0, collection.Length);
    //    T toRemove = collection[index];

    //    T[] tempCollection = new T[collection.Length - 1];
    //    for (int i = 0; i < collection.Length; i++)
    //    {
    //        if (!collection[i].Equals(toRemove))
    //        {
    //            tempCollection[i] = collection[i];
    //        }
    //    }
    //    tempCollection = collection.ConvertToList().RemoveNullReferences().ToArray();
    //    collection = tempCollection;
    //    return toRemove;
    //}

    public static List<T> ConvertToList<T>(this T[] array)
    {
        List<T> list = new List<T>();
        foreach (T element in array)
        {
            list.Add(element);
        }
        return list;
    }

    public static List<T> RemoveNullReferences<T>(this List<T> list)
    {
        int c = 0;
        int count = list.Count;
        while (c < count)
        {
            if (list[c] == null)
            {
                list.RemoveAt(c);
                count--;
            }
            else
            {
                c++;
            }
        }
        return list;
    }


    /// <summary>
    /// Removes a random element from a collection and returns the removed element, updating the collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static T RemoveFrom<T>(this List<T> collection)
    {
        int index = Random.Range(0, collection.Count);
        T toRemove = collection[index];
        collection.Remove(toRemove);
        return toRemove;
    }

    /// <summary>
    /// Remove a specific element from the collection and return it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static T RemoveFrom<T>(this List<T> collection, T element)
    {
        collection.Remove(element);
        return element;
    }


    public static string PrintCollection<T>(this T[] collection)
    {
        string s = "";
        foreach (T element in collection)
        {
            if (element == null)
            {
                s += "null, ";
            }
            else
            {
                s += element.ToString() + ", ";
            }
        }
        return s;
    }

    public static string PrintCollection<T>(this List<T> collection, string deliniation = "")
    {
        string s = "";
        foreach (T element in collection)
        {
            if (element == null)
            {
                s += "null" + deliniation;
            }
            else
            {
                s += element.ToString() + deliniation;
            }
        }
        return s;
    }
}

public static class UsefulStuff
{
    public static IEnumerator DoAfter(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public static KeyCode ToKeyCode(this string s)
    {
        switch(s.ToLower())
        {
            case "a": return KeyCode.A;
            case "b": return KeyCode.B;
            case "c": return KeyCode.C;
            case "d": return KeyCode.D;
            case "e": return KeyCode.E;
            case "f": return KeyCode.F;
            case "g": return KeyCode.G;
            case "h": return KeyCode.H;
            case "i": return KeyCode.I;
            case "j": return KeyCode.J;
            case "k": return KeyCode.K;
            case "l": return KeyCode.L;
            case "m": return KeyCode.M;
            case "n": return KeyCode.N;
            case "o": return KeyCode.O;
            case "p": return KeyCode.P;
            case "q": return KeyCode.Q;
            case "r": return KeyCode.R;
            case "s": return KeyCode.S;
            case "t": return KeyCode.T;
            case "u": return KeyCode.U;
            case "v": return KeyCode.V;
            case "w": return KeyCode.W;
            case "x": return KeyCode.X;
            case "y": return KeyCode.Y;
            case "z": return KeyCode.Z;
            case "0": return KeyCode.Alpha0;
            case "1": return KeyCode.Alpha1;
            case "2": return KeyCode.Alpha2;
            case "3": return KeyCode.Alpha3;
            case "4": return KeyCode.Alpha4;
            case "5": return KeyCode.Alpha5;
            case "6": return KeyCode.Alpha6;
            case "7": return KeyCode.Alpha7;
            case "8": return KeyCode.Alpha8;
            case "9": return KeyCode.Alpha9;
            default: return KeyCode.Alpha0;
        }
    }
}


public static class MathExtensionsC
{
    public static int[] GetOrderedArray(this int i)
    {
        int[] orderedArray = new int[i];
        
        for (int k = 0; k < i; k++)
        {
            orderedArray[k] = k;
        }
        Debug.Log(orderedArray.PrintCollection());
        return orderedArray;
    }

    public static int PlusOrMinus(this int i)
    {
        return i * Random.Range(0, 2) * 2 - 1;
    }

    public static float PlusOrMinus(this float i)
    {
        return i * (Random.Range(0, 2) * 2 - 1);
    }

    /*
     * DIVISIBILTY OF INTS
     */
    public static int[] GetDivisibility(this int i)
    {
        List<int> _divisibles = i.GetDivisibilityList();

        //Convert to array
        return _divisibles.ToArray();
    }


    public static List<int> GetDivisibilityList(this int i)
    {
        List<int> _divisibles = new List<int>();

        for (int x = 1; x < Mathf.Infinity; x++)
        {
            if (x > i) { break; }
            if (CheckDivisibility(i, x))
            {
                _divisibles.Add(x);
            }
        }

        return _divisibles;
    }

    public static bool CheckDivisibility(this int i, int divisor)
    {
        return i % divisor == 0;
    }




    public static Vector3Int ToVector3(this int i)
    {
        return new Vector3Int(i, i, i);
    }

    public static Vector3 ToVector3(this float f)
    {
        return new Vector3(f, f, f);
    }

    public static Vector2Int ToVector2(this int i)
    {
        return new Vector2Int(i, i);
    }

    public static Vector2 ToVector2(this float f)
    {
        return new Vector2(f, f);
    }
}

public static class Vector3ExtensionsC
{
    public static Vector2 To2D(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }
}


public static class TerrainExtensionsC
{
    public static TerrainData GetCurrentTerrainData(this Terrain terrain)
    {
        if (terrain != null)
        {
            return terrain.terrainData;
        }
        return default(TerrainData);
    }

    public static Vector3 GetTerrainSize(this Terrain terrain)
    {
        if (terrain != null)
        {
            return terrain.terrainData.size;
        }
        return Vector3.zero;
    }
}


public static class RendererExtensionsC
{
    public static void SetColor(this Renderer renderer, Color color)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        block.SetColor("_Color", color);
        renderer.SetPropertyBlock(block);
    }

    public static bool IsOnScreen(this GameObject g, Camera camera)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(g.transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}



public static class StringExtensionsC
{
    public static string RemoveRandomIndexC(this string s)
    {
        return s.Remove(Random.Range(0, s.Length));
    }

    public static string RandomizeStringC(this string s)
    {
        string S = "";
        //add letters to list
        List<char> letters = new List<char>();
        for (int i = 0; i < s.Length; i++)
        {
            letters.Add(s[i]);
        }

        int L = letters.Count;
        while (L > 0)
        {
            char randomLetter = letters[Random.Range(0, letters.Count)];

            S += randomLetter;
            letters.Remove(randomLetter);
            L--;
        }
        return S;
    }
}


public static class TransformExtensionsC
{
    public static Transform[] GetImmediateChildArray(this Transform t)
    {
        Transform[] array = new Transform[t.childCount];
        for (int i = 0; i < t.childCount; i++)
        {
            array[i] = t.GetChild(i);
        }
        return array;
    }
}


public static class ColorExtensionsC
{
    public static Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1));
    }

    public static Color GetRainbowColor(int index = -1)
    {
        Color[] colors = {new Color(1f, 0f, 0f)
                        , new Color(1f, .5f, 0f)
                        , new Color(1f, 1f, 0f)
                        , new Color(.5f, 1f, 0f)
                        , new Color(0f, 1f, 0f)
                        , new Color(0f, 1f, .5f)
                        , new Color(0f, 1f, 1f)
                        , new Color(0f, .5f, 1f)
                        , new Color(0f, 0f, 1f)
                        , new Color(.5f, 0f, 1f)
                        , new Color(1f, 0f, 1f)
                        , new Color(1f, 0f, .5f)};

        if (index < 0)
        {
            return colors[Random.Range(0, colors.Length)];
        }
        return colors[index % colors.Length];
    }
}