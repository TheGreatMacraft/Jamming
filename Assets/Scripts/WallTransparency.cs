using UnityEngine;

public class WallTransparency : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public float Alpha = 0.2f;

    bool playerInside = false;
    float time = 0.0f;
    float prevAlpha = 1.0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Color color = spriteRenderer.color;
        if (playerInside)
            color.a = Mathf.Lerp(prevAlpha, Alpha, time * 2.0f);
        else
            color.a = Mathf.Lerp(prevAlpha, 1.0f, time * 2.0f);

        spriteRenderer.color = color;
        time += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            time = 0.0f;
            prevAlpha = spriteRenderer.color.a;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            time = 0.0f;
            prevAlpha = spriteRenderer.color.a;
        }
    }
}
