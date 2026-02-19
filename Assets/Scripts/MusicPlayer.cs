using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public double silencePadding = 0.05f;
    
    public AudioClip introClip;
    public AudioClip loopClip;

    public AudioSource introSource;
    public AudioSource loopSource;

    void Start()
    {
        double startTime = AudioSettings.dspTime;
        
        introSource.clip = introClip;
        introSource.loop = false;
        
        loopSource.clip = loopClip;
        loopSource.loop = true;
        
        introSource.PlayScheduled(startTime);
        
        loopSource.PlayScheduled(startTime + introClip.length + silencePadding);
    }
}