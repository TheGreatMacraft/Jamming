using System;
using System.Linq;
using UnityEngine;

public class RoomsLayout : MonoBehaviour
{
    public RoomBounds[] Rooms;

    public static RoomsLayout Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Bounds GetRoomBounds(RoomType roomType)
    {
        return Rooms.First(room => room.RoomType == roomType).BoxCollider.bounds;
    }

    public bool IsPointInRoom(RoomType roomType, Vector2 point)
    {
        Bounds bounds = GetRoomBounds(roomType);
        return bounds.Contains(point);
    }
}

[Serializable]
public class RoomBounds
{
    public RoomType RoomType;
    public BoxCollider2D BoxCollider;
}

[Serializable]
public enum RoomType
{
    DiningRoom,
    LivingRoom,
    Garage,
    Bathroom,
    Bedroom
}
