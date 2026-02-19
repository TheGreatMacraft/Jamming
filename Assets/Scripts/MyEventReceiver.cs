using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MyEventReceiver : MonoBehaviour
{
    private AudioSource audioSource;
    
    public SoundType soundType;
    public float volume = 1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlaySound()
    {
        SoundManager.PlaySound(soundType, volume, audioSource);
    }
}