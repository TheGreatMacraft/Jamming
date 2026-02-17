using UnityEngine;

public class WarningFlash : MonoBehaviour
{
    public float TimeOn = 1.0f;
    public float TimeOff = 1.0f;

    float timer = 0.0f;

    SpriteRenderer spriteRenderer;

    public bool Visible => spriteRenderer.enabled;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        timer = TimeOn;
        spriteRenderer.enabled = true;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0.0f)
        {
            if (spriteRenderer.enabled)
            {
                spriteRenderer.enabled = false;
                timer = TimeOff;
            }
            else
            {
                spriteRenderer.enabled = true;
                timer = TimeOn;
            }
        }
    }
}
