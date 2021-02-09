using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    public Sound[] sounds;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        current = this;

        foreach (Sound s in sounds)
        {
            if (s != null)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
            }
        }
    }

    public void Play(string name, float pitch = 1f)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.pitch = pitch + UnityEngine.Random.Range(0f, .05f);
            s.source.Play();
        }
        catch
        {
            Debug.LogWarning($"Sound {name} nicht gefunden!");
        }
    }
}
