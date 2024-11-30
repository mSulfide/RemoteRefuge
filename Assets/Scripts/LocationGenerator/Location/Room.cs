using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Vector3 _size;
    
    private List<Transition> _transitions = new();
    private List<Transition> _connected = new();

    public static Room GetPrefab(ERoom room) => Resources.Load<Room>($"Rooms/{room}");

    private void OnEnable()
    {
        foreach(var marker in GetComponentsInChildren<TransitionMarker>())
            _transitions.Add(new(marker));
    }

    private void OnDisable()
    {
        _transitions.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new(0f, 1f, 0f, 0.8f);
        Vector3 position = transform.position + Vector3.up * _size.y / 2;
        Gizmos.DrawWireCube(position, _size);
    }
}