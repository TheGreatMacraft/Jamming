using UnityEngine;

public class RotateableItem : Item
{
    public Sprite SpriteLeft;
    public Sprite SpriteRight;
    public Sprite SpriteUp;
    public Sprite SpriteDown;

    public void Rotate()
    {
        Sprite currentSprite = SpriteRenderer.sprite;
        if (currentSprite == SpriteLeft) SpriteRenderer.sprite = SpriteUp;
        else if (currentSprite == SpriteUp) SpriteRenderer.sprite = SpriteRight;
        else if (currentSprite == SpriteRight) SpriteRenderer.sprite = SpriteDown;
        else if (currentSprite == SpriteDown) SpriteRenderer.sprite = SpriteLeft;
    }
}
