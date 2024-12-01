using UnityEngine;

[ExecuteInEditMode]
public class GroundCheck : MonoBehaviour
{
    [Tooltip("Maximum distance from the ground.")]
    [SerializeField] private float _distanceThreshold = .15f;

    [Tooltip("Whether this transform is grounded now.")]
    public bool IsGrounded = true;
    /// <summary>Called when the ground is touched again.</summary>
    public event System.Action Grounded;

    private const float OriginOffset = .001f;
    private Vector3 RaycastOrigin => transform.position + Vector3.up * OriginOffset;
    private float RaycastDistance => _distanceThreshold + OriginOffset;

    private void LateUpdate()
    {
        bool isGroundedNow = Physics.Raycast(RaycastOrigin, Vector3.down, _distanceThreshold * 2);

        if (isGroundedNow && !IsGrounded)
            Grounded?.Invoke();

        IsGrounded = isGroundedNow;
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(RaycastOrigin, RaycastOrigin + Vector3.down * RaycastDistance, IsGrounded ? Color.white : Color.red);
    }
}
