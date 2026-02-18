using UnityEngine;

public class WallCollisionFix : MonoBehaviour
{
    public Collider2D ColliderExtendedUp;
    public Collider2D ColliderExtendedDown;
    public float YOffset;

    bool playerAbove;

    private void Awake()
    {
        ColliderExtendedUp.enabled = false;
        ColliderExtendedDown.enabled = false;
    }

    private void Update()
    {
        bool playerAboveNow = Player.Instance.transform.position.y > transform.position.y + YOffset;
        if (playerAboveNow && !playerAbove)
        {
            ColliderExtendedUp.enabled = false;
            ColliderExtendedDown.enabled = true;
        }
        else if (!playerAboveNow && playerAbove)
        {
            ColliderExtendedDown.enabled = false;
            ColliderExtendedUp.enabled = true;
        }

        playerAbove = playerAboveNow;
    }
}
