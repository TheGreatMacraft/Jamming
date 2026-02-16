using UnityEngine;

public class Item : MonoBehaviour
{
    public float Weight;

    [HideInInspector] public bool IsPlaced = true;
    public bool IsBeingCarried => !IsPlaced;

    public Transform PickupPosition;
    [HideInInspector] public SpriteRenderer SpriteRenderer;

    new Rigidbody2D rigidbody;

    bool justDropped = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        int carrySortingOrder = IsBeingCarried ? 150 : 0;
        SpriteRenderer.sortingOrder = Util.CalcSortingOrder(transform.position.y) + carrySortingOrder;
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
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.isTrigger == false)
                collider.enabled = false;
        }
        IsPlaced = false;
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
    }
}
