using Fusion;
using Fusion.Addons.Physics;
using System;
using UnityEngine;

public class MovementComponent : NetworkBehaviour
{
    [Header("Speed")]
    [SerializeField] float _defaultSpeed = 5f;
    [SerializeField] float _crouchSpeed = 3f;
    [SerializeField] float _sprintSpeed = 8f;
    [SerializeField] float _jumpForce = 5f;
    [Networked] public float _currentSpeed { get; set; }

    [Header("References")]
    [SerializeField] NetworkRigidbody3D _rb;

    public event Action<float> OnMoving = delegate {};
    public event Action OnJumping = delegate { };
    public event Action<bool> OnCrouching = delegate { };
    public event Action OnInteracting = delegate { };

    private void Awake()
    {
        _currentSpeed = _defaultSpeed;
    }

    public void Movement(Vector3 direction, NetworkRunner runner)
    {
        Vector3 velocity = direction.normalized * _currentSpeed;
        _rb.Rigidbody.MovePosition(_rb.Rigidbody.position + velocity * runner.DeltaTime);

        OnMoving?.Invoke(Mathf.Clamp01(direction.magnitude) * _currentSpeed);
    }

    public void Jump(bool isGrounded)
    {
        if (!isGrounded) return;

        //_rb.Rigidbody.linearVelocity = Vector3.up * _jumpForce;
        //_rb.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

        Vector3 velocity = _rb.Rigidbody.linearVelocity;
        velocity.y = _jumpForce;
        _rb.Rigidbody.linearVelocity = velocity;

        OnJumping?.Invoke();
    }

    public void SetCrouch(bool isCrouching)
    {
        _currentSpeed = isCrouching ? _crouchSpeed : _defaultSpeed;
        OnCrouching?.Invoke(isCrouching);
    }

    public void SetSprint(bool isSprinting)
    {
        _currentSpeed = isSprinting ? _sprintSpeed : _defaultSpeed;
    }
}