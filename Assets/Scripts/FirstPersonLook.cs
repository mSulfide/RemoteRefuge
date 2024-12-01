using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private Transform _character;
    private float _sensitivity = 2;
    private float _smoothing = 1.5f;

    private Vector2 _velocity;
    private Vector2 _frameVelocity;

    private void Reset()
    {
        _character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector2 mouseDelta = new(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * _sensitivity);
        _frameVelocity = Vector2.Lerp(_frameVelocity, rawFrameVelocity, 1 / _smoothing);
        _velocity += _frameVelocity;
        _velocity.y = Mathf.Clamp(_velocity.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-_velocity.y, Vector3.right);
        _character.localRotation = Quaternion.AngleAxis(_velocity.x, Vector3.up);
    }
}
