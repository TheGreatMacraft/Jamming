using UnityEngine;

public class WalkableSpaceRequirement : Requirement
{
    public Collider2D SpaceNeededCollider;

    Collider2D[] queryResults = new Collider2D[1];

    public override bool IsSatisfied(Item item)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;
        int count = Physics2D.OverlapCollider(SpaceNeededCollider, contactFilter, queryResults);
        return count == 0;
    }
}
