using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(AudioSource))]
public class GameOverSequence : MonoBehaviour
{
    public static GameOverSequence Instance;

    public AudioSource thisSource;
    public AudioSource musicSource;

    public AudioClip gameOverClip;
    public float waitAfterClip = 0f;

    public PixelPerfectCamera pixelPerfectCamera;
    public int finalZoom;

    public SmoothFollow smoothFollowScript;
    public Transform businessman;
    public float newCameraFollowSpeed;

    public float waitBeforeMadAnimation;
    public Animator businessmanAnimator;
    
    public int timesYapping;
    public int timesJumping;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        thisSource = GetComponent<AudioSource>();
        thisSource.playOnAwake = false;

        Debug.Log("Pixel Perfect Camera is bugged - can't be assigned in inspector");
    }

    private void Update()
    {
        if (smoothFollowScript.Target == businessman && smoothFollowScript.IsOnTarget)
            StartCoroutine(BusinessmanTransition());
    }

    public void GameOver()
    {
        DisableObjects();

        StartCoroutine(PlayGameOverSound());
    }

    private void DisableObjects()
    {
        musicSource.Stop();
        businessman.GetComponent<BusinessMan>().enabled = false;
    }

    private IEnumerator PlayGameOverSound()
    {
        thisSource.PlayOneShot(gameOverClip);
        yield return new WaitForSeconds(gameOverClip.length + waitAfterClip);

        ZoomToBusinessman();
    }

    private void ZoomToBusinessman()
    {
        smoothFollowScript.Target = businessman;
        smoothFollowScript.Speed = newCameraFollowSpeed;
    }

    private IEnumerator BusinessmanTransition()
    {
        yield return new WaitForSeconds(waitBeforeMadAnimation);

        yield return PlayAnimationSequence();
    }
    
    private IEnumerator PlayAnimationSequence()
    {
        yield return PlayAnimation("GetMad", 1);
        
        yield return PlayAnimation("MadYapper", timesYapping);
        
        yield return PlayAnimation("Jump", timesJumping);
    }

    private IEnumerator PlayAnimation(string animationName, int timesRepeated)
    {
        businessmanAnimator.Play(animationName, 0, 0f);
        var clip = GetAnimationClip(animationName);

        if (clip == null)
        {
            Debug.LogWarning("Clip not found: " + animationName);
            yield break;
        }

        float duration = clip.length * timesRepeated;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private AnimationClip GetAnimationClip(string animationName)
    {
        foreach (AnimationClip clip in businessmanAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip;
            }
        }
        
        return null;
    }

}
    
    

