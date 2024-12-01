using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [Header("Running")]
    [SerializeField] private bool _canRun = true;
    [SerializeField] private float _runSpeed = 9;
    [SerializeField] private KeyCode _runningKey = KeyCode.LeftShift;

    public bool IsRunning { get; private set; }

    private Rigidbody _rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        IsRunning = _canRun && Input.GetKey(_runningKey);

        float targetMovingSpeed = IsRunning ? _runSpeed : _speed;
        if (speedOverrides.Count > 0)
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();

        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * targetMovingSpeed;

        _rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, _rigidbody.velocity.y, targetVelocity.y);
    }
}