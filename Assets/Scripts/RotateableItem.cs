using UnityEngine;

public class RotateableItem : Item
{
    public SpriteRenderer SpriteLeft;
    public SpriteRenderer SpriteRight;
    public SpriteRenderer SpriteUp;
    public SpriteRenderer SpriteDown;

    protected override void Awake()
    {
        base.Awake();
        if (SpriteLeft.gameObject.activeSelf)       { SpriteRenderer = SpriteLeft; }
        else if (SpriteUp.gameObject.activeSelf)    { SpriteRenderer = SpriteUp; }
        else if (SpriteRight.gameObject.activeSelf) { SpriteRenderer = SpriteRight; }
        else if (SpriteDown.gameObject.activeSelf)  { SpriteRenderer = SpriteDown; }
    }

    public void Rotate()
    {
        SpriteRenderer nextSpriteRenderer = null;

        if (SpriteLeft.gameObject.activeSelf)       { nextSpriteRenderer = SpriteUp;    SpriteLeft.gameObject.SetActive(false); }
        else if (SpriteUp.gameObject.activeSelf)    { nextSpriteRenderer = SpriteRight; SpriteUp.gameObject.SetActive(false); }
        else if (SpriteRight.gameObject.activeSelf) { nextSpriteRenderer = SpriteDown;  SpriteRight.gameObject.SetActive(false); }
        else if (SpriteDown.gameObject.activeSelf)  { nextSpriteRenderer = SpriteLeft;  SpriteDown.gameObject.SetActive(false); }

        nextSpriteRenderer.gameObject.SetActive(true);
        SpriteRenderer = nextSpriteRenderer;
    }
}
