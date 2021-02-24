using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class allows for colors of various object types to be set by way of a switch statement.
/// Alpha value of colored object is preserved.
/// </summary>
public class Colorizer : MonoBehaviour
{
    public enum ColorizeType { MeshRenderer, MeshRenderer_Emissive, SpriteRenderer, Image, Light, TextMeshProUGUI, ParticleSystem }

    [System.Serializable]
    public class ColorizeSet
    {
        [SerializeField] public ColorizeType type;
        [SerializeField] public GameObject[] gameObject;
    }

    [Header("Configure Sets")]
    [SerializeField] ColorizeSet[] colorizeSets = null;

    public virtual void Recolor(Color newColor)
    {
        if (colorizeSets.Length <= 0) { return; }
        foreach (ColorizeSet colorSet in colorizeSets)
        {
            Color _color = newColor;

            foreach (GameObject objectToColor in colorSet.gameObject)
            {
                switch (colorSet.type)
                {
                    case ColorizeType.Image:
                        Image i = objectToColor.GetComponent<Image>();
                        if(i!= null)
                        {
                            _color.a = i.color.a;
                            i.color = _color;
                        }
                        break;
                    case ColorizeType.MeshRenderer:
                        MeshRenderer mr = objectToColor.GetComponent<MeshRenderer>();
                        if (mr != null)
                        {
                            if(mr.material != null)
                            {
                                _color.a = mr.material.color.a;
                            }
                            mr.SetColor(_color);
                        }
                        break;
                    case ColorizeType.MeshRenderer_Emissive:
                        MeshRenderer mre = objectToColor.GetComponent<MeshRenderer>();
                        if(mre != null)
                        {
                            mre.material.SetColor("_EmissionColor", _color);
                        }
                        break;
                    case ColorizeType.SpriteRenderer:
                        SpriteRenderer sr = objectToColor.GetComponent<SpriteRenderer>();
                        if(sr != null)
                        {
                            _color.a = sr.color.a;
                            sr.color = _color;
                        }

                        break;
                    case ColorizeType.Light:
                        Light l = objectToColor.GetComponent<Light>();
                        if(l!= null)
                        {
                            _color.a = l.color.a;
                            l.color = _color;
                        }
                        break;
                    case ColorizeType.TextMeshProUGUI:
                        TextMeshProUGUI t = objectToColor.GetComponent<TextMeshProUGUI>();
                        if(t!= null)
                        {
                            _color.a = t.color.a;
                            t.color = _color;
                        }
                        break;
                    case ColorizeType.ParticleSystem:
                        ParticleSystem ps = objectToColor.GetComponent<ParticleSystem>();
                        if(ps != null)
                        {
                            var main = ps.main;
                            main.startColor = _color;
                            StartCoroutine(ColorParticles(_color, ps));
                        }

                        break;
                }
            }
        }
    }

    public IEnumerator ColorParticles(Color _color, params ParticleSystem[] particles)
    {
        if (particles != null)
        {
            foreach (ParticleSystem particle in particles)
            {
                var main = particle.main;
                main.startColor = _color;
            }
        }
        yield return null;
    }
}
