using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;                 // A name for the sound clip.
    public AudioClip clip;              // The audio clip.

    [Range(0.0f, 1.0f)]
    public float volume;                // The volume for the clip.
    [Range(0.1f, 3.0f)]
    public float picth;                 // The pitch for the clip.
}
