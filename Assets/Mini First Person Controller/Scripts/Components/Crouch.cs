using UnityEngine;

public class Crouch : MonoBehaviour
{
    [SerializeField] private KeyCode _key = KeyCode.C;

    [Header("Slow Movement")]
    [Tooltip("Movement to slow down when crouched.")]
    [SerializeField] private FirstPersonMovement _movement;
    [Tooltip("Movement speed when crouched.")]
    [SerializeField] private float _movementSpeed = 2;

    [Header("Low Head")]
    [Tooltip("Head to lower when crouched.")]
    [SerializeField] private Transform _headToLower;
    [SerializeField] private float _crouchYHeadPosition = 1;
    [Tooltip("Collider to lower when crouched.")]
    [SerializeField] private CapsuleCollider _colliderToLower;

    public bool IsCrouched { get; private set; }
    public event System.Action CrouchStart, CrouchEnd;

    private float? _defaultHeadYLocalPosition;
    private float? _defaultColliderHeight;


    private void Reset()
    {
        _movement = GetComponentInParent<FirstPersonMovement>();
        _headToLower = _movement.GetComponentInChildren<Camera>().transform;
        _colliderToLower = _movement.GetComponentInChildren<CapsuleCollider>();
    }

    private void LateUpdate()
    {
        if (Input.GetKey(_key))
        {
            if (_headToLower)
            {
                if (!_defaultHeadYLocalPosition.HasValue)
                    _defaultHeadYLocalPosition = _headToLower.localPosition.y;

                _headToLower.localPosition = new(_headToLower.localPosition.x, _crouchYHeadPosition, _headToLower.localPosition.z);
            }

            if (_colliderToLower)
            {
                if (!_defaultColliderHeight.HasValue)
                    _defaultColliderHeight = _colliderToLower.height;

                float loweringAmount;
                loweringAmount = _defaultHeadYLocalPosition.HasValue ? _defaultHeadYLocalPosition.Value - _crouchYHeadPosition : _defaultColliderHeight.Value * .5f;

                _colliderToLower.height = Mathf.Max(_defaultColliderHeight.Value - loweringAmount, 0);
                _colliderToLower.center = Vector3.up * _colliderToLower.height * .5f;
            }

            // Set IsCrouched state.
            if (!IsCrouched)
            {
                IsCrouched = true;
                SetSpeedOverrideActive(true);
                CrouchStart?.Invoke();
            }
        }
        else
        {
            if (IsCrouched)
            {
                if (_headToLower)
                    _headToLower.localPosition = new(_headToLower.localPosition.x, _defaultHeadYLocalPosition.Value, _headToLower.localPosition.z);

                if (_colliderToLower)
                {
                    _colliderToLower.height = _defaultColliderHeight.Value;
                    _colliderToLower.center = Vector3.up * _colliderToLower.height * .5f;
                }

                IsCrouched = false;
                SetSpeedOverrideActive(false);
                CrouchEnd?.Invoke();
            }
        }
    }


    #region Speed override.
    private void SetSpeedOverrideActive(bool state)
    {
        if(!_movement)
            return;

        if (state)
            if (!_movement.speedOverrides.Contains(SpeedOverride))
                _movement.speedOverrides.Add(SpeedOverride);
        else
            if (_movement.speedOverrides.Contains(SpeedOverride))
                _movement.speedOverrides.Remove(SpeedOverride);
    }

    private float SpeedOverride() => _movementSpeed;
    #endregion
}
