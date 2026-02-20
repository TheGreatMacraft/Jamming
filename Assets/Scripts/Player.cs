using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public float MoveSpeed;
    public CircleCollider2D PickupReachCollider;
    public SpriteRenderer PlayerSpriteRenderer;
    public Animator PlayerAnimator;

    public Transform CarryPosition;

    public SpriteRenderer PlaceholderSpriteRenderer;

    public float WeightSpeedReduction = 1.0f;

    new Rigidbody2D rigidbody;

    Item carriedItem = null;
    bool movedSincePickup = false;
    Vector2 originalPickupPosition;

    MoveDirection moveDirection = MoveDirection.Right;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Instance = this;
    }

    private void OnDisable()
    {
        rigidbody.linearVelocity = Vector2.zero;

        if (carriedItem == null) PlayerAnimator.Play("Idle");
        else PlayerAnimator.Play("Squished Idle");
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
        rigidbody.linearVelocity = MoveSpeed * weightMultiplier * input.normalized;

        if (input.y > 0.5f) moveDirection = MoveDirection.Up;
        else if (input.y < -0.5f) moveDirection = MoveDirection.Down;
        if (input.x > 0.5f) moveDirection = MoveDirection.Right;
        else if (input.x < -0.5f) moveDirection = MoveDirection.Left;

        if (carriedItem != null)
        {
            Vector2 pickupOffset = carriedItem.PickupPosition.position - carriedItem.transform.position;
            carriedItem.transform.position = CarryPosition.position - (Vector3)pickupOffset;

            PlaceholderSpriteRenderer.transform.position = GetItemDropPosition();

            if (Keyboard.current.rKey.wasPressedThisFrame && carriedItem is RotateableItem rotateableItem)
            {
                rotateableItem.Rotate();
                PlaceholderSpriteRenderer.sprite = carriedItem.SpriteRenderer.sprite;
            }
        }

        if (input != Vector2.zero)
            movedSincePickup = true;
        PlaceholderSpriteRenderer.enabled = (input == Vector2.zero && movedSincePickup);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (carriedItem == null)
                PickupClosestItem();
            else
                DropItem();
        }

        if (input == Vector2.zero && carriedItem == null) PlayerAnimator.Play("Idle");
        else if (input == Vector2.zero && carriedItem != null) PlayerAnimator.Play("Squished Idle");
        else if (input != Vector2.zero && carriedItem == null) PlayerAnimator.Play("Walk");
        else if (input != Vector2.zero && carriedItem != null) PlayerAnimator.Play("Squished Walk");

        if (moveDirection == MoveDirection.Left) PlayerSpriteRenderer.flipX = true;
        else if (moveDirection == MoveDirection.Right) PlayerSpriteRenderer.flipX = false;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            BusinessMan bman = FindFirstObjectByType<BusinessMan>();
            if (Vector2.Distance(bman.transform.position, transform.position) < PickupReachCollider.radius)
            {
                Item item = bman.SpawnNewItem();
                if (item != null)
                    PickupItem(item);
            }
        }
    }

    void PickupClosestItem()
    {
        Vector2 pickupLocalCenter = (Vector2)PickupReachCollider.transform.localPosition + PickupReachCollider.offset;
        Vector2 pickupCenter = PickupReachCollider.localToWorldMatrix.MultiplyPoint(pickupLocalCenter);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pickupCenter, PickupReachCollider.radius);

        Item closestItem = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            Item item = collider.GetComponentInParent<Item>();
            if (item != null)
            {
                float distance = Vector2.Distance(item.transform.position, pickupCenter);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }
        }

        if (closestItem != null)
            PickupItem(closestItem);
    }

    void PickupItem(Item item)
    {
        if (carriedItem != null)
            DropItem();

        movedSincePickup = false;
        originalPickupPosition = item.transform.position;

        carriedItem = item;
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
        Vector2 direction = moveDirection switch
        {
            MoveDirection.Left => Vector2.left,
            MoveDirection.Right => Vector2.right,
            MoveDirection.Up => Vector2.up,
            MoveDirection.Down => Vector2.down,
            _ => Vector2.zero
        };

        direction.x *= 1.2f + carriedItem.SpriteRenderer.bounds.extents.x;
        direction.y *= 1.2f + carriedItem.SpriteRenderer.bounds.extents.y / 2.0f;

        Vector2 position = (Vector2)transform.position + direction;
        return position;
    }
}
