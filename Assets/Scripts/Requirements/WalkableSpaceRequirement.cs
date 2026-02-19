using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkableSpaceRequirement : Requirement
{
    public bool RequiredAll;

    Collider2D[] spaceNeededColliders;

    List<Collider2D> queryResults = new();

    private void Awake()
    {
        spaceNeededColliders = GetComponentsInChildren<Collider2D>();
    }

    public override bool IsSatisfied(Item item)
    {
        if (RequiredAll)
            return spaceNeededColliders.All(CheckCollider);
        else
            return spaceNeededColliders.Any(CheckCollider);
    }

    private bool CheckCollider(Collider2D spaceNeededCollider)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;

        queryResults.Clear();
        int count = Physics2D.OverlapCollider(spaceNeededCollider, contactFilter, queryResults);

        foreach (Collider2D collider in queryResults)
        {
            if (Util.IsTagOnParent(collider.gameObject, "Player"))
                continue;

            var wallCollider = collider.GetComponentInParent<WallCollisionFix>();
            if (wallCollider != null && wallCollider.ColliderDefault != null)
            {
                if (wallCollider.ColliderDefault.bounds.Intersects(spaceNeededCollider.bounds))
                    return false;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
