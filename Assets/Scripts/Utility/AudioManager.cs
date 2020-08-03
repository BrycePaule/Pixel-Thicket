using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioMixerGroup _master;
    [SerializeField] private AudioMixerGroup _music;
    [SerializeField] private AudioMixerGroup _ambience;
    [SerializeField] private AudioMixerGroup _sfx;

    public Sound[] Sounds;

    private Dictionary<SoundType, AudioSource[]> _soundDict = new Dictionary<SoundType, AudioSource[]>();
    private Dictionary<SoundGroup, AudioMixerGroup> _mixerDict = new Dictionary<SoundGroup, AudioMixerGroup>();
    
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

        BuildMixerDictionary();
        BuildSourceDictionary();

    }

    public void Play(SoundType soundType, float volume = 1, bool playRandom = false)
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
                newSource.outputAudioMixerGroup = _mixerDict[s.SoundGoup];
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

    private void BuildMixerDictionary()
    {
        _mixerDict[SoundGroup.Master] = _master;
        _mixerDict[SoundGroup.Music] = _music;
        _mixerDict[SoundGroup.Ambience] = _ambience;
        _mixerDict[SoundGroup.SFX] = _sfx;
    }

}
