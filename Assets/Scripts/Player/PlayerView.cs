using Fusion;
using UnityEngine;

public class PlayerView : NetworkBehaviour
{
    [SerializeField] private GameObject _playerVisual;
    [SerializeField] CharacterAnimationController _animationController;
    [SerializeField] CharacterColliderResizer _characterColliderResizer;
    [SerializeField] MovementComponent _movement;
    [SerializeField] HealthSystem _healthSystem;

    public override void Spawned()
    {    
        _movement.OnMoving += MoveAnimation;
        _movement.OnJumping += JumpAnimation;
        _movement.OnCrouching += OnCrouchAnimation;

        _healthSystem.OnDead += () => EnableMeshRender(true);
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

    /*void EnableMeshRender(bool isDead)
    {
        _playerVisual.SetActive(!isDead);
    }*/
}