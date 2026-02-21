using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public enum SoundType
{
    FOOTSTEP,
    LIFT,
    PLACE,
    BUSINESSMAN_TALKING,
    BUSINESSMAN_MAD
}

[System.Serializable]
public class SoundGroup
{
    public SoundType type;
    public List<AudioClip> sounds;
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    private Random rnd = new Random();

    [SerializeField] private List<SoundGroup> soundGroups;
    private Dictionary<SoundType, List<AudioClip>> soundDict;
    
    public AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        audioSource = GetComponent<AudioSource>();
        
        soundDict = new Dictionary<SoundType, List<AudioClip>>();

        foreach (var group in soundGroups)
            soundDict[group.type] = group.sounds;
    }

    public static void PlaySound(SoundType soundType, AudioSource newSource = null)
    {
        int soundCount = instance.soundDict[soundType].Count;
        
        int index = instance.rnd.Next(soundCount);
        
        AudioSource source = newSource?? instance.audioSource;
        
        source.PlayOneShot(instance.soundDict[soundType][index]);
    }
    
    public static void PlaySound(AudioClip audioClip, AudioSource newSource = null)
    {
        AudioSource source = newSource?? instance.audioSource;
        
        source.PlayOneShot(audioClip);
    }
}