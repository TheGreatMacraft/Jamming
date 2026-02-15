using UnityEngine;

public class Item : MonoBehaviour
{
    public float Weight;

    [HideInInspector] public bool IsPlaced = true;
    public bool IsBeingCarried => !IsPlaced;

    public Transform PickupPosition;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPickup()
    {
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.isTrigger == false)
                collider.enabled = false;
        }
        IsPlaced = false;
    }

    public void OnDrop()
    {
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.isTrigger == false)
                collider.enabled = true;
        }
        IsPlaced = true;
    }
}
