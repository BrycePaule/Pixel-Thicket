using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null)
                {
                    _instance = new AudioManager();
                }
            }
            return _instance;
        }
    }
        
    private void Awake()
    {
        if (_instance != null) { Destroy(this.gameObject); }

        foreach (Sound s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
            s.Source.playOnAwake = s.PlayOnAwake;
        }
    }

    public void Play(SoundTypes soundType, float volume = 1)
    {

        // AudioSource sound = _soundDict[soundType].Source;
        AudioSource s = Array.Find(Sounds, sound => sound.SoundType == soundType).Source;
        if (Sounds == null) { return; }
        s.volume = volume;
        s.Play();
    }

}
