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

    private Dictionary<SoundType, List<AudioSource>> _soundDict = new Dictionary<SoundType, List<AudioSource>>();
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

    private void Start()
    {
        foreach (var val in _soundDict[SoundType.PlayerHit])
        {
            print(val);
        }
    }

    public AudioSource PlayInstant(SoundType soundType, float volume = 1f, float pitch = 1f, bool playRandom = false)
    {
        List<AudioSource> sources;

        if (_soundDict.ContainsKey(soundType))
        {
            sources = _soundDict[soundType];
        }
        else
        {
            throw new KeyNotFoundException("<" + soundType + "> not found in sound library.");
        }


        if (playRandom)
        {
            List<int> rnd = new List<int>();
            for (int n = 0; n < sources.Count; n++)
            {
                rnd.Add(n);
            }

            for (int i = 0; i < rnd.Count; i++)
            {
                int index = Random.Range(0, rnd.Count);

                AudioSource source = sources[index];
                
                if (source == null) { continue; }
                if (source.isPlaying) 
                { 
                    rnd.RemoveAt(index);
                    continue; 
                }
               
                float prevV = source.volume;
                float prevP = source.pitch;

                source.volume = prevV * volume;
                source.pitch = prevP * pitch;

                source.Play();
                
                source.volume = prevV;
                source.pitch = prevP;

                return source;
            }
        }

        else
        {
            for (int i = 0; i < sources.Count; i++)
            {
                AudioSource source = sources[i];
                if (source == null) { continue; }
                if (source.isPlaying) { continue; }

                float prevV = source.volume;
                float prevP = source.pitch;

                source.volume = prevV * volume;
                source.pitch = prevP * pitch;

                source.Play();

                source.volume = prevV;
                source.pitch = prevP;

                return source;
            }
        }

        return null;
    }

    public void Stop(AudioSource source, float seconds = 0)
    {
        // if (seconds == 0)
        if (true)
        {
            source.Stop();
        }
        else
        {
            StartCoroutine(Lower(source, seconds));
        }
    }

    private IEnumerator Lower(AudioSource source, float seconds)
    {
        float stepsPerSecond = 50;
        float prevVolume = source.volume;

        for (int i = 0; i < stepsPerSecond; i++)
        {
            source.volume -= source.volume * ((stepsPerSecond / seconds) * 100f);
            yield return new WaitForSeconds(seconds / stepsPerSecond);
        }

        source.volume = prevVolume;
        source.Stop();
    }

    private void BuildSourceDictionary()
    {
        foreach (Sound sound in Sounds)
        {
            List<AudioSource> audioSources = new List<AudioSource>();
            
            for (int i = 0; i < sound.Counts; i++)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = sound.Clip;
                audioSource.outputAudioMixerGroup = _mixerDict[sound.SoundGoup];
                audioSource.volume = sound.ImportVolume;
                audioSource.pitch = sound.ImportPitch;
                audioSource.loop = sound.Loop;
                audioSource.playOnAwake = sound.PlayOnAwake;

                audioSources.Add(audioSource);
            }

            sound.Sources = audioSources;

            if (_soundDict.ContainsKey(sound.SoundType))
            {
                foreach (AudioSource source in audioSources)
                {
                    _soundDict[sound.SoundType].Add(source);
                }
            }

            else
            {
                _soundDict[sound.SoundType] = audioSources;
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
