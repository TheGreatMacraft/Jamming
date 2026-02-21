using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MyEventReceiver : MonoBehaviour
{
    private AudioSource audioSource;
    
    public SoundType soundType;
    public SoundType secondarySoundType;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlaySound(string audioName)
    {
        if(audioName != null)
            SoundManager.PlaySound(Resources.Load<AudioClip>("MadAudio/" +  audioName));
        else
            SoundManager.PlaySound(soundType, audioSource);
    }

    private void PlaySecondary()
    {
        SoundManager.PlaySound(secondarySoundType, audioSource);
    }
}