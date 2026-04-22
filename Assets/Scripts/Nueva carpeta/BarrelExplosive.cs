using Fusion;
using System;
using UnityEngine;

public class BarrelExplosive : NetworkBehaviour, IDamageable
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionHeight;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _damage;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private LayerMask _layerMask;
    [Networked] public float _currentHealth { get; private set; }
    Collider[] _targets = new Collider[10];

    public override void Spawned()
    {
        _currentHealth = _maxHealth;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(float damage)
    {
        if (!HasStateAuthority) return;

        Debug.Log("Barrel took damage: " + damage);

        if (damage <= 0) return;

        _currentHealth = MathF.Max(_currentHealth - damage, 0);

        if (_currentHealth <= 0)
        {
            //Explode();
            Runner.Despawn(Object);
        }
    }

    /*private void Explode()
    {
        int count = Runner.GetPhysicsScene().OverlapSphere(transform.position, _explosionRadius, _targets, _layerMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < count; i++)
        {
            var col = _targets[i];

            if (col.TryGetComponent<NetworkBehaviour>(out var netObj))
            {
                if (netObj is IDamageable damageable)
                {
                    damageable.RPC_TakeDamage(_damage);
                }
            }
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}