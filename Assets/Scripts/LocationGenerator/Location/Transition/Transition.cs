using UnityEngine;

public class Transition
{
    private Vector3 _localPosition;
    private Vector3 _direction;

    public Vector3 Direction => _direction;

    public Transition(TransitionMarker marker)
    {
        _localPosition = marker.LocalPosition;
        _direction = marker.Direction;
    }

    public Vector3 MoveTo(Vector3 globalPosition) => globalPosition - _localPosition;
}