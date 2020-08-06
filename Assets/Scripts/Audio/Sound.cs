using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;

    public SoundType SoundType;
    public SoundGroup SoundGoup;
    public int StackCount;

    [Range(0f, 1f)] public float ImportVolume;
    [Range(0.1f, 3f)] public float ImportPitch;
    public bool Loop;
    public bool PlayOnAwake;

    public AudioSource[] Sources;

}
