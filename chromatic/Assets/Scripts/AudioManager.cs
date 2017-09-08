using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour 
{
    public Sound[] sounds;              // The array of all non-position dependent sounds in the game.

    /* Called just before Start(). */
    private void Awake()
    {
        // Setup a new audio source for each sound in the sounds array.
        for (int i = 0; i < sounds.Length; ++i)
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.clip = sounds[i].clip;
            newAudioSource.volume = sounds[i].volume;
            newAudioSource.pitch = sounds[i].picth;
            newAudioSource.loop = sounds[i].loop;
            sounds[i].audioSource = newAudioSource;
        }
    }

    /* Use this for initialization. */
    private void Start() 
	{
        //Play("Background");
	}
	
	/* Plays a given sound clip. */
    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        s.audioSource.Play();
    }
}
