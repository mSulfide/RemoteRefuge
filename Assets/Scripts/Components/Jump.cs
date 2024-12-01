using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float _jumpStrength = 2;
    [Tooltip("Prevents jumping when the transform is in mid-air.")]
    [SerializeField] private GroundCheck _groundCheck;

    public event System.Action Jumped;

    private Rigidbody _rigidbody;

    private void Reset()
    {
        _groundCheck = GetComponentInChildren<GroundCheck>();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (Input.GetButtonDown("Jump") && (!_groundCheck || _groundCheck.IsGrounded))
        {
            _rigidbody.AddForce(Vector3.up * 100 * _jumpStrength);
            Jumped?.Invoke();
        }
    }
}
