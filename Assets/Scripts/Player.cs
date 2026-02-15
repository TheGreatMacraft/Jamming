using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float MoveSpeed;
    public CircleCollider2D PickupReachCollider;

    public Transform CarryPosition;
    public Transform ItemDropPosition;

    new Rigidbody2D rigidbody;

    Item carriedItem = null;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 input = new Vector2(0, 0);
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed) input.x -= 1.0f;
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) input.x += 1.0f;
        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed) input.y -= 1.0f;
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed) input.y += 1.0f;

        float WeightedSpeed = MoveSpeed;
        if (carriedItem != null)
        {
            
        }
        rigidbody.linearVelocity = input.normalized * MoveSpeed;


        if (carriedItem != null)
        {
            Vector2 pickupOffset = carriedItem.PickupPosition.position - carriedItem.transform.position;
            carriedItem.transform.position = CarryPosition.position - (Vector3)pickupOffset;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (carriedItem == null)
                PickupItem();
            else
                DropItem();
        }
    }

    void PickupItem()
    {
        if (carriedItem != null)
            DropItem();

        Vector2 pickupLocalCenter = (Vector2)PickupReachCollider.transform.localPosition + PickupReachCollider.offset;
        Vector2 pickupCenter = PickupReachCollider.localToWorldMatrix.MultiplyPoint(pickupLocalCenter);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pickupCenter, PickupReachCollider.radius);

        Item closestItem = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<Item>(out Item item))
            {
                float distance = Vector2.Distance(item.transform.position, pickupCenter);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }
        }

        if (closestItem == null)
            return;

        carriedItem = closestItem;
        carriedItem.OnPickup();
    }

    void DropItem()
    {
        carriedItem.transform.position = ItemDropPosition.position;
        carriedItem.OnDrop();
        carriedItem = null;
    }
}
