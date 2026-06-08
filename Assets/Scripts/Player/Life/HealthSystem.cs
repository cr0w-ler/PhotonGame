using Fusion;
using System;
using UnityEngine;

public class HealthSystem : NetworkBehaviour
{
    [SerializeField] int _maxLife;
    [Networked] public float _currentHealth { get; private set; }
    [Networked] public bool _isDead { get; private set; }

    public event Action OnDead = delegate { };
    public event Action<float> OnHealthChanged = delegate { };

    public override void Spawned()
    {
        InitializeHealth();
    }

    void InitializeHealth()
    {
        _currentHealth = _maxLife;
    }

    void TakeDamage(float damage)
    {
        if (!HasStateAuthority) return;

        if (damage <= 0) return;

        _currentHealth = MathF.Max(_currentHealth - damage, 0);

        if (_currentHealth <= 0)
        {
            _isDead = true;
            OnDead();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(float damage)
    {
        TakeDamage(damage);
    }
}