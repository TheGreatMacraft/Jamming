using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float MoveSpeed;
    public CircleCollider2D PickupReachCollider;
    public SpriteRenderer PlayerSpriteRenderer;
    public Animator PlayerAnimator;

    public Transform CarryPosition;
    public Transform ItemDropPositionLeft;
    public Transform ItemDropPositionRight;
    public Transform ItemDropPositionUp;
    public Transform ItemDropPositionDown;

    public SpriteRenderer PlaceholderSpriteRenderer;

    public float WeightSpeedReduction = 1.0f;

    new Rigidbody2D rigidbody;

    Item carriedItem = null;
    bool movedSincePickup = false;
    Vector2 originalPickupPosition;

    MoveDirection moveDirection = MoveDirection.Right;

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

        float weight = (carriedItem != null) ? carriedItem.Weight : 0.0f;
        float weightMultiplier = 1.0f - (weight/100.0f) * WeightSpeedReduction;
        rigidbody.linearVelocity = input.normalized * MoveSpeed * weightMultiplier;

        if (input.x > 0.5f) moveDirection = MoveDirection.Right;
        else if (input.x < -0.5f) moveDirection = MoveDirection.Left;
        if (input.y > 0.5f) moveDirection = MoveDirection.Up;
        else if (input.y < -0.5f) moveDirection = MoveDirection.Down;

        if (carriedItem != null)
        {
            Vector2 pickupOffset = carriedItem.PickupPosition.position - carriedItem.transform.position;
            carriedItem.transform.position = CarryPosition.position - (Vector3)pickupOffset;

            PlaceholderSpriteRenderer.transform.position = GetItemDropPosition();
        }

        if (input != Vector2.zero)
            movedSincePickup = true;
        PlaceholderSpriteRenderer.enabled = (input == Vector2.zero && movedSincePickup);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (carriedItem == null)
                PickupItem();
            else
                DropItem();
        }

        PlayerSpriteRenderer.sortingOrder = Util.CalcSortingOrder(transform.position.y);

        if (input == Vector2.zero && carriedItem == null) PlayerAnimator.Play("Idle");
        else if (input == Vector2.zero && carriedItem != null) PlayerAnimator.Play("Squished Idle");
        else if (input != Vector2.zero && carriedItem == null) PlayerAnimator.Play("Walk");
        else if (input != Vector2.zero && carriedItem != null) PlayerAnimator.Play("Squished Walk");
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

        movedSincePickup = false;
        originalPickupPosition = closestItem.transform.position;

        carriedItem = closestItem;
        carriedItem.OnPickup();

        PlaceholderSpriteRenderer.sprite = carriedItem.SpriteRenderer.sprite;
        PlaceholderSpriteRenderer.transform.localScale = carriedItem.SpriteRenderer.transform.localScale;
        PlaceholderSpriteRenderer.transform.rotation = carriedItem.SpriteRenderer.transform.rotation;
    }

    void DropItem()
    {
        if (!movedSincePickup)
            carriedItem.transform.position = originalPickupPosition;
        else
            carriedItem.transform.position = GetItemDropPosition();

        carriedItem.OnDrop();
        carriedItem = null;

        PlaceholderSpriteRenderer.sprite = null;
    }

    Vector2 GetItemDropPosition()
    {
        Transform dropTransform = moveDirection switch
        {
            MoveDirection.Left => ItemDropPositionLeft,
            MoveDirection.Right => ItemDropPositionRight,
            MoveDirection.Up => ItemDropPositionUp,
            MoveDirection.Down => ItemDropPositionDown,
            _ => null
        };
        return dropTransform.position;
    }
}
