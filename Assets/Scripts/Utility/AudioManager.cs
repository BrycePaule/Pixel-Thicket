using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sound[] Sounds;
    
    private Dictionary<SoundTypes, Sound> _soundDict = new Dictionary<SoundTypes, Sound>();

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

        FillSoundsDictionary();
    }

    public void Play(SoundTypes soundType)
    {
        _soundDict[soundType].Source.Play();
        // Sounds[0].Source.Play();
    }

    private void FillSoundsDictionary()
    {
        _soundDict.Add(SoundTypes.Rain, Sounds[0]);
        _soundDict.Add(SoundTypes.Explosion, Sounds[1]);
    }

}
