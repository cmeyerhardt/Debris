using UnityEngine;
using UnityEngine.Audio;

public class AudioMix : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer mixer = null;
    [SerializeField] GameObject fx = null;

    [Header("Snapshots")]
    [SerializeField] AudioMixerSnapshot mainSnapshot = null;        //main snapshot
    [SerializeField] AudioMixerSnapshot fxSnapshot = null;          //snapshot used when fx are active
    [SerializeField] AudioMixerSnapshot windowSnapshot = null;      //window snapshot
    [SerializeField] AudioMixerSnapshot windowFXSnapshot = null;    //window snapshot when fx are active
    [SerializeField] AudioMixerSnapshot loseSnapshot = null;        //player has lost game

    private void Awake()
    {
        if(mixer != null)
        {

            SetSnapshot(true);
        }
        else
        {
            // According to the API, AudioMixer "is a singleton representing a specific audio mixer asset in the project." 
            // Can AudioMixer be referenced in code in the case of null or must is always be referenced in the inspector?
            Debug.LogWarning("Audio Mixer not found.");
        }
    }

    public void SetSnapshot(bool gamePlaying)
    {
        AudioMixerSnapshot snapshot = null;
        if (fx != null && fx.activeInHierarchy)
        {
            if (gamePlaying) 
            {
                snapshot = fxSnapshot;
            }
            else
            {
                snapshot = windowFXSnapshot;
            }
        }
        else
        {
            if (gamePlaying)
            {
                snapshot = mainSnapshot;
            }
            else
            {
                snapshot = windowSnapshot;
            }
        }
        if (snapshot != null) 
        {
            snapshot.TransitionTo(1f);
        }
    }

    public void SetLoseSnapshot()
    {
        if(loseSnapshot != null)
        {
            loseSnapshot.TransitionTo(2f);
        }
    }
}
