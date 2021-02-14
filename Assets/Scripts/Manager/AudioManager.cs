using System;
using UnityEngine;

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
                s.source.playOnAwake = false;
            }
        }
    }

    public void Play(string name, float pitch = 1,  bool playVariablePitch = true)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (playVariablePitch)
            {
                s.source.pitch = pitch + UnityEngine.Random.Range(0f, .05f);
            }
            s.source.Play();
        }
        catch
        {
            Debug.LogWarning($"Sound {name} nicht gefunden!");
        }
    }
}
