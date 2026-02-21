using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
            if (wallCollider != null)
            {
                if (wallCollider.DefaultBounds.Intersects(spaceNeededCollider.bounds))
                {
                    //Debug.Log("Fail Walkable space requirement on " + AnimationUtility.CalculateTransformPath(collider.transform, transform.root) + " " + collider.name);
                    return false;
                }
            }
            else
            {
                //Debug.Log("Fail Walkable space requirement on " + AnimationUtility.CalculateTransformPath(collider.transform, transform.root) + " " + collider.name);
                return false;
            }
        }
        return true;
    }
}
