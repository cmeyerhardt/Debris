using System.Collections;
using UnityEngine;

/// <summary>
/// This class will color the startColor of a Particle System using a gradient created from two colors, retrieved from RandomColorScheme class
/// </summary>
public class SetParticleColors : MonoBehaviour
{
    [SerializeField] int minColorIndex = 0;
    [SerializeField] int maxColorIndex = 2;

    private void Start()
    {
        SetColors();
    }

    private void SetColors()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ColorSchemePicker rcs = FindObjectOfType<ColorSchemePicker>();

        if (ps == null || rcs == null) { Destroy(this); return; }

        Gradient g = new Gradient();
        //GradientColorKey key1 = new GradientColorKey(rcs.GetColor(minColorIndex), 0f);
        //GradientColorKey key2 = new GradientColorKey(rcs.GetColor(maxColorIndex), 1f);

        GradientColorKey key1 = new GradientColorKey(ColorScheme.GetColor(minColorIndex), 0f);
        GradientColorKey key2 = new GradientColorKey(ColorScheme.GetColor(maxColorIndex), 1f);

        GradientAlphaKey alphaKey1 = new GradientAlphaKey(1f, 0f);
        GradientAlphaKey alphaKey2 = new GradientAlphaKey(1f, 1f);

        g.SetKeys(new GradientColorKey[2] { key1, key2 }, new GradientAlphaKey[2] { alphaKey1, alphaKey2 });
        
        StartCoroutine(ColorParticles(g, ps));
    }

    public IEnumerator ColorParticles(Gradient g, params ParticleSystem[] particles)
    {
        if (particles != null)
        {
            foreach (ParticleSystem particle in particles)
            {
                var main = particle.main;
                main.startColor = g;
            }
        }
        yield return null;
        Destroy(this);
    }
}
