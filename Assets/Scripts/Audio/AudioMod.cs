using System.Collections.Generic;
using UnityEngine;

public class AudioMod : MonoBehaviour
{
    [System.Serializable]
    public class AudioClipBlock
    {
        public string label = "";
        public AudioClip[] audioClip = null;
        [Range(0f, 1f)] public float maxVolume = 1f;
        [Range(0f, 1f)] public float randomizePitchAmount = 0f;
        [Range(0f, 1f)] public float randomizePitchIncrement = 0f;
        [Range(-3f, 3f)] public float defaultPitchValue = 1f;
        [Range(-3f, 3f)] public float maxPitchValue = 3f;
        public bool useVector3Zero = false;
        //public bool playOneShot = false;
    }

    //AudioSource audioSource = null;
    [SerializeField] AudioClipBlock onEnableAudioClipBlock = null;
    [SerializeField] AudioClipBlock onDisableAudioClipBlock = null;
    [SerializeField] AudioClipBlock[] audioClipBlocks = null;
    [SerializeField] AudioSource audioSource = null;

    //Queue<int> recentlyPlayed = new Queue<int>();

    //List<GameObject> playing = new List<GameObject>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && audioClipBlocks.Length > 0)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (audioSource != null)
        {
            audioSource.maxDistance = 100f;
        }
    }

    private void OnEnable()
    {
        if(onEnableAudioClipBlock != null && onEnableAudioClipBlock.audioClip != null && onEnableAudioClipBlock.audioClip.Length > 0)
            PlayAudio(onEnableAudioClipBlock);
    }

    //private void OnDisable()
    //{
    //    if (onDisableAudioClipBlock != null && onDisableAudioClipBlock.audioClip != null && onDisableAudioClipBlock.audioClip.Length > 0)
    //        PlayAudio(onDisableAudioClipBlock);
    //}

    private bool PlayAudio(AudioClipBlock audioBlock)
    {
        if (audioBlock != null)
        {
            if (audioBlock.audioClip.Length > 0)
            {
                float pitch = audioBlock.defaultPitchValue;
                if (audioBlock.randomizePitchAmount > 0f)
                {
                    float upperLimit = Mathf.Min(audioBlock.defaultPitchValue + audioBlock.randomizePitchAmount, audioBlock.maxPitchValue);
                    float lowerLimit = Mathf.Max(audioBlock.defaultPitchValue - audioBlock.randomizePitchAmount, 0f);

                    //randomize pitch by increments * semi-random amount
                    if (audioBlock.randomizePitchIncrement > 0f)
                    {
                        float increment = audioBlock.randomizePitchAmount * audioBlock.randomizePitchIncrement;//audioClipBlocks[i].randomizePitchAmount / audioClipBlocks[i].defaultPitchValue * audioClipBlocks[i].randomizePitchIncrement;
                        int maxIncrements = (int)(audioBlock.randomizePitchAmount / increment);
                        int numIncrements = Random.Range(0, maxIncrements + 1);

                        pitch = Mathf.Clamp(
                            audioBlock.defaultPitchValue * (1 + (numIncrements * increment)).PlusOrMinus(),
                            lowerLimit,
                            upperLimit);
                    }
                    else //randomize pitch within limits
                    {
                        pitch = Mathf.Clamp(
                            audioBlock.defaultPitchValue + Random.Range(0, audioBlock.randomizePitchAmount).PlusOrMinus() / pitch,
                            lowerLimit,
                            upperLimit);
                    }
                }

                //audioSource.clip = audioBlock.audioClip[Random.Range(0, audioBlock.audioClip.Length)];
                AudioClip clip = audioBlock.audioClip[Random.Range(0, audioBlock.audioClip.Length)];

                if (enabled && clip != null)
                {
                    if(audioSource != null)
                    {
                        Debug.Log(name + " Playing " + audioBlock.label);
                        audioSource.clip = clip;
                        audioSource.pitch = pitch;
                        audioSource.volume = audioBlock.maxVolume;
                        audioSource.Play();
                    }
                    return true;
                }
            }
        }
        return false;
    }

    //unused
    public void PlayAudioClip(string labelName)
    {
        //todo -- this method will be slower than referencing the index directly
        //can possibly store audioBlock data in a static collection for faster lookups if this method call became commonly used
        for(int i = 0; i < audioClipBlocks.Length; i++)
        {
            if (audioClipBlocks[i].label.ToLower() == labelName.ToLower())
            {
                PlayAudioClip(i);
            }
        }
    }

    public void PlayAudioClip(int i)
    {
        if(audioClipBlocks.Length <= 0 || i > audioClipBlocks.Length - 1) { return; }
        PlayAudio(audioClipBlocks[i]);

        //if (!recentlyPlayed.Contains(i))
        //{
        //    if(PlayAudio(audioClipBlocks[i]))
        //    {
        //        recentlyPlayed.Enqueue(i);
        //        Invoke("RemoveFromQueue", .1f);
        //    }
        //}
        //else
        //{
        //    //Debug.Log(gameObject.name + "Audio recently played: " + audioClipBlocks[i].label);
        //}
    }

    //private void RemoveFromQueue()
    //{
    //    if (recentlyPlayed.Count > 0)
    //        recentlyPlayed.Dequeue();
    //}
}
