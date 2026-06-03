using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    [SerializeField] float _maxLife = 100f;
    private float _currentLife;
    [SerializeField] float _speed = 5f;

    [SerializeField] NetworkRigidbody3D _rb;
    [SerializeField] NetworkMecanimAnimator _mecanimAnimator;

    [Header("Attack")]
    [SerializeField] float _damage = 25f;
    bool _isAttacking;


    [SerializeField]float stoppingDistance = 2f;
    Player _player;
    float sqrtDistance;

    public override void Spawned()
    {
        _currentLife = _maxLife;
    }

    public override void FixedUpdateNetwork()
    {
        sqrtDistance = (_player.transform.position - _rb.Rigidbody.position).sqrMagnitude;

        if (sqrtDistance > stoppingDistance * stoppingDistance)
        {
            MoveTo();
        }
        else
        {
            Attack();
        }
    }

    public void SetTarget(Player player)
    {
        _player = player;
    }

    private void Attack()
    {
        if (_isAttacking) return;

        _mecanimAnimator.Animator.SetBool(AnimParams.Attack, true);

        SetBoolAttacking(true);
    }

    public void ApplyDamage()
    {
        _player.RPC_TakeDamage(_damage);
    }

    void MoveTo()
    {
        if(_player == null) return;

        Vector3 direction = _player.transform.position - _rb.Rigidbody.position;

        _rb.Rigidbody.MovePosition(_rb.Rigidbody.position + direction.normalized * _speed * Runner.DeltaTime);

        _mecanimAnimator.Animator.SetFloat(AnimParams.Speed, _speed);
    }

    public void SetBoolAttacking(bool attacking)
    {
        _isAttacking = attacking;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TakeDamage(float damage)
    {
        TakeDamage(damage);
    }

    private void TakeDamage(float damage)
    {
        if(damage <= 0) return;

        _currentLife = Mathf.Max(_currentLife - damage, 0);

        if (_currentLife <= 0)
        {
            Runner.Despawn(Object);
        }
    }
}