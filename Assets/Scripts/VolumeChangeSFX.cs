using System;
using UnityEngine;

public class VolumeChangeSFX : MonoBehaviour
{
    private void Update()
    {
        GetComponent<AudioSource>().volume = SoundManager.instance.audioSource.volume;
    }
}