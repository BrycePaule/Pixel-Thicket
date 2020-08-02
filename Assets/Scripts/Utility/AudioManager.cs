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

    public void Play(SoundTypes soundType, float volume = 1, bool playRandom = false)
    {
        AudioSource[] sources;
        try
        {
            sources = _soundDict[soundType];
        }
        catch
        {
            throw new KeyNotFoundException("<" + soundType + "> not found in sound library.");
        }

        if (playRandom)
        {
            List<int> rnd = new List<int>();
            for (int n = 0; n < sources.Length; n++)
            {
                rnd.Add(n);
            }

            for (int i = 0; i < rnd.Count; i++)
            {
                int index = Random.Range(0, rnd.Count);

                AudioSource source = sources[index];
                
                if (source == null) { continue; }
                
                if (!source.isPlaying)
                { 
                    float currV = source.volume;

                    source.volume = currV * volume;
                    source.Play();
                    print(source.volume);

                    source.volume = currV;
                    return;
                }

                rnd.RemoveAt(index);
            }
        }

        else
        {
            for (int i = 0; i < sources.Length; i++)
            {
                AudioSource source = sources[i];
                if (!source.isPlaying)
                {
                    float currV = source.volume;
                    source.volume = currV * volume;
                    source.Play();
                    source.volume = currV;
                    return;
                }
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
                newSource.volume = s.ImportVolume;
                newSource.pitch = s.ImportPitch;
                newSource.loop = s.Loop;
                newSource.playOnAwake = s.PlayOnAwake;

                sources[i] = newSource;
            }

            s.Sources = sources;

            bool soundExists = _soundDict.ContainsKey(s.SoundType);
            if (soundExists)
            {
                AudioSource[] prevSources = _soundDict[s.SoundType];
                AudioSource[] newSources = new AudioSource[s.StackCount + prevSources.Length];

                for (int i = 0; i < prevSources.Length; i++)
                {
                    newSources[i] = prevSources[i];
                }

                for (int j = 0; j < sources.Length - 1 ; j++)
                {
                    newSources[prevSources.Length + j] = sources[j];
                }

                _soundDict.Remove(s.SoundType);
                _soundDict.Add(s.SoundType, newSources);
                continue;
            }

            else
            {
                _soundDict.Add(s.SoundType, sources);
            }

        }
    }
}
