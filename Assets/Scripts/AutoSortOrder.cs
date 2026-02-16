using UnityEngine;

public class AutoSortOrder : MonoBehaviour
{
    public float YOffset = 0.0f;
    public bool UpdateEveryFrame = true;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSortingOrder();
    }

    void Update()
    {
        if (UpdateEveryFrame)
            UpdateSortingOrder();
    }

    public void UpdateSortingOrder()
    {
        float y = transform.position.y + YOffset;
        spriteRenderer.sortingOrder = -Mathf.RoundToInt(y * 100.0f);
    }
}
