using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameOverSequence : MonoBehaviour
{
    public static GameOverSequence Instance;

    public Player playerScript;

    public AudioSource thisSource;
    public AudioSource musicSource;

    public AudioClip gameOverClip;
    public float waitAfterClip = 0f;

    public GameObject camera;
    public int finalZoom;

    public SmoothFollow smoothFollowScript;
    public Transform businessman;
    public float newCameraFollowSpeed;

    private bool transitionStarted = false;
    
    public float waitBeforeMadAnimation;
    public Animator businessmanAnimator;
    
    public int timesYapping;
    public int timesJumping;

    public int repeatSequence;

    public Image BlackScreen;
    public float blackScreenFade;

    [SerializeField] public Canvas[] allUnwantedUI;
    [SerializeField] public GameObject[] walls;

    public GameObject finalCamera;

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
        if (smoothFollowScript.Target == businessman && smoothFollowScript.IsOnTarget && !transitionStarted)
            StartCoroutine(BusinessmanTransition());
    }

    public void GameOver()
    {
        DisableObjects();
        
        foreach (var canvas in allUnwantedUI)
        {
            canvas.enabled = false;
        }

        StartCoroutine(PlayGameOverSound());
    }

    private void DisableObjects()
    {
        musicSource.Stop();
        businessman.GetComponent<BusinessMan>().enabled = false;
        playerScript.enabled = false;
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
        transitionStarted = true;
        
        yield return new WaitForSeconds(waitBeforeMadAnimation);
        StartCoroutine(WaitForBlackScreen());

        yield return PlayAnimation("GetMad", 1);
    }


    private IEnumerator PlayAnimation(string animationName, int timesRepeated)
    {
        businessmanAnimator.Play(animationName);
        
        var clip = GetAnimationClip(animationName);
       yield return  new WaitForSeconds(clip.length * timesRepeated);
       
       if(animationName == "GetMad" || animationName == "Jump")
           yield return PlayAnimation("MadYapper", timesYapping);
       else if(animationName == "MadYapper")
           yield return PlayAnimation("Jump", timesJumping);
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

    private IEnumerator WaitForBlackScreen()
    {
        Debug.Log("Waiting");
        float time = GetAnimationClip("GetMad").length + (GetAnimationClip("MadYapper").length * timesYapping + GetAnimationClip("Jump").length * timesJumping) * repeatSequence;
        yield return new WaitForSeconds(time);
        
        Debug.Log("Fading in");
        StartCoroutine(FadeInImage(BlackScreen,blackScreenFade));
    }
    
    private IEnumerator FadeInImage(Image image, float duration)
    {
        Color color = image.color;
        
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(timer / duration);
            image.color = color;
            yield return null;
        }
        
        color.a = 1f;
        image.color = color;
        
        PrepareForScreenShoot();
        
        BlackScreen.enabled = false;
        
        finalCamera.SetActive(true);
        camera.SetActive(false);
    }

    private void PrepareForScreenShoot()
    {
        foreach (var wall in walls)
        {
            wall.GetComponentInChildren<WallTransparency>().enabled = false;
            
            Color color = wall.GetComponentInChildren<SpriteRenderer>().color;
            color.a = 0.55f;
            wall.GetComponentInChildren<SpriteRenderer>().color = color;
        }
    }
}
    
    

