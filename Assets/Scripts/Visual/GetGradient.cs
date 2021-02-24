using UnityEngine;

public class GetGradient : MonoBehaviour
{
    [SerializeField] GameObject[] ps = null;

    void Start()
    {
        ChangeGradient();
    }

    private void ChangeGradient()
    {
        //ColorSchemePicker rcs = FindObjectOfType<ColorSchemePicker>();

        if (/*rcs != null &&*/ ps.Length > 0)
        {
            foreach (GameObject g in ps)
            {
                if(g != null)
                {
                    ParticleSystem pss = g.GetComponent<ParticleSystem>();
                    if (pss != null)
                    {
                        var main = pss.main;
                        Gradient gradient = ColorScheme.GetGradient();
                        if(gradient != null)
                        {
                            main.startColor = gradient;
                        }
                    }
                }
            }
        }

        Destroy(this);
    }
}

