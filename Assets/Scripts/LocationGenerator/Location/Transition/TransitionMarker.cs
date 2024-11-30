using UnityEngine;

public class TransitionMarker : MonoBehaviour
{
    public Vector3 LocalPosition => transform.localPosition;

    public Vector3 Direction => transform.forward;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}