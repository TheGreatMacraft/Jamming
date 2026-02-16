using UnityEngine;

public class Item : MonoBehaviour
{
    public float Weight;

    [HideInInspector] public bool IsPlaced = true;
    public bool IsBeingCarried => !IsPlaced;

    public Transform PickupPosition;
    [HideInInspector] public SpriteRenderer SpriteRenderer;

    new Rigidbody2D rigidbody;
    AutoSortOrder autoSortOrder;

    bool justDropped = false;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        autoSortOrder = GetComponent<AutoSortOrder>();
    }

    private void FixedUpdate()
    {
        if (justDropped && rigidbody.IsSleeping())
        {
            justDropped = false;
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void OnPickup()
    {
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        rigidbody.linearVelocity = Vector2.zero;
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.isTrigger == false)
                collider.enabled = false;
        }
        IsPlaced = false;
        autoSortOrder.YOffset = -1.5f;
    }

    public void OnDrop()
    {
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.isTrigger == false)
                collider.enabled = true;
        }
        IsPlaced = true;
        justDropped = true;
        autoSortOrder.YOffset = 0.0f;
    }
}
