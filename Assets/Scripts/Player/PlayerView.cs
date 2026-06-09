using Fusion;
using UnityEngine;

public class PlayerView : NetworkBehaviour
{
    [SerializeField] private GameObject _playerVisual;
    [SerializeField] CharacterAnimationController _animationController;
    [SerializeField] CharacterColliderResizer _characterColliderResizer;
    [SerializeField] MovementComponent _movement;
    [SerializeField] HealthSystem _healthSystem;
    [SerializeField] Ragdoll _ragdoll;

    public override void Spawned()
    {
        _ragdoll.DisableRagdoll();

        _movement.OnMoving += MoveAnimation;
        _movement.OnJumping += JumpAnimation;
        _movement.OnCrouching += OnCrouchAnimation;

        _healthSystem.OnDeadChanged += OnDeadStateChanged;
        _healthSystem.OnRespawn += OnRespawned;
    }

    void MoveAnimation(float speed)
    {
        _animationController.SetFloat(AnimParams.Speed, speed);
    }

    void JumpAnimation()
    {
        _animationController.SetTrigger(AnimParams.Jump);
    }

    public void OnCrouchAnimation(bool isCrouching)
    {
        if(isCrouching)
        {
            _characterColliderResizer.SetSize(1, new Vector3(0, -0.5f, 0));
            _animationController.SetBool(AnimParams.Crouch, true);
        }
        else
        {
            _characterColliderResizer.SetSize(2, new Vector3(0, 0, 0));
            _animationController.SetBool(AnimParams.Crouch, false);
        }
    }

    void OnDeadStateChanged(bool isDead)
    {
        if (isDead)
            _ragdoll.ActivateRagdoll();
        else
            _ragdoll.DisableRagdoll();
    }

    void OnRespawned()
    {
        _ragdoll.DisableRagdoll();
    }
}