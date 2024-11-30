using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationGenerator : MonoBehaviour
{
    [SerializeField] private int _roomCount = 3;

    private readonly List<Room> _rooms = new();
    private Room _prefab;

    [ContextMenu(nameof(Generate))]
    public void Generate()
    {
        ClearRooms();
        Room prefab = Room.GetPrefab(ERoom.TestRoom);
        _rooms.Add(Instantiate(prefab, transform));
    }

    private void AddRoom()
    {

    }

    private void ClearRooms()
    {
        _rooms.ForEach(room => Destroy(room.gameObject));
        _rooms.Clear();
    }
}
