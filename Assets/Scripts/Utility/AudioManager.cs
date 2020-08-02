using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    private Dictionary<SoundTypes, AudioSource[]> _soundDict = new Dictionary<SoundTypes, AudioSource[]>();
    
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

        BuildSourceDictionary();

    }

    public void Play(SoundTypes soundType, float volume = 1)
    {
        AudioSource[] sources = _soundDict[soundType];

        if (sources == null) { return; }

        for (int i = 0; i < sources.Length; i++)
        {
            // print(i + " " + sources[i].isPlaying);
            if (!sources[i].isPlaying)
            {
                sources[i].volume = volume;
                sources[i].Play();
                return;
            }
        }
    }

    private void BuildSourceDictionary()
    {
        foreach (Sound s in Sounds)
        {
            AudioSource[] sources = new AudioSource[s.StackCount];

            for (int i = 0; i < s.StackCount; i++)
            {
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = s.Clip;
                newSource.volume = s.Volume;
                newSource.pitch = s.Pitch;
                newSource.loop = s.Loop;
                newSource.playOnAwake = s.PlayOnAwake;

                sources[i] = newSource;
            }

            s.Sources = sources;
            _soundDict.Add(s.SoundType, sources);
        }
    }
}
