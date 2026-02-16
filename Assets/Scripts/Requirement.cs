using System;
using UnityEngine;

public abstract class Requirement : ScriptableObject
{
    public abstract bool IsSatisfied(Item item);

    public string Description;
}


[CreateAssetMenu(fileName="InRoomRequirement", menuName="Requirements/InRoom")]
public class InRoomRequirement : Requirement
{
    public RoomType RoomType;

    public override bool IsSatisfied(Item item)
    {
        return RoomsLayout.Instance.IsPointInRoom(RoomType, item.transform.position);
    }
}

[CreateAssetMenu(fileName = "NotInRoomRequirement", menuName = "Requirements/NotInRoom")]
public class NotInRoomRequirement : Requirement
{
    public RoomType RoomType;

    public override bool IsSatisfied(Item item)
    {
        return !RoomsLayout.Instance.IsPointInRoom(RoomType, item.transform.position);
    }
}

[CreateAssetMenu(fileName = "NextToRequirement", menuName = "Requirements/NextTo")]
public class NextToRequirement : Requirement
{
    public string NextToTag;
    public float MaxDistance = 1.5f;

    public override bool IsSatisfied(Item item)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(NextToTag);
        foreach (GameObject obj in taggedObjects)
        {
            if (Vector2.Distance(obj.transform.position, item.transform.position) <= MaxDistance)
                return true;
        }
        return false;
    }
}

[CreateAssetMenu(fileName = "NotNextToRequirement", menuName = "Requirements/NotNextTo")]
public class NotNextToRequirement : Requirement
{
    public string NextToTag;
    public float MinDistance = 1.5f;

    public override bool IsSatisfied(Item item)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(NextToTag);
        foreach (GameObject obj in taggedObjects)
        {
            if (Vector2.Distance(obj.transform.position, item.transform.position) < MinDistance)
                return false;
        }
        return true;
    }
}

// TODO: 
// - against the wall
// - walkable space
