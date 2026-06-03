using Fusion;
using Fusion.Addons.Physics;
using System;
using UnityEngine;

public class BarrelExplosive : NetworkBehaviour, IDamageable
{
    [SerializeField] GameObject _explosionVFX;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionHeight;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _damage;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] int _capacityArray = 50;
    [Networked] public float _currentHealth { get;  set; }
    [Networked] private NetworkBool _exploded { get; set; }
    Collider[] _targets;

    public override void Spawned()
    {
        _currentHealth = _maxHealth;
        _targets = new Collider[_capacityArray];
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(float damage)
    {
        if (_exploded) return;

        if (damage <= 0) return;

        _currentHealth = MathF.Max(_currentHealth - damage, 0);

        if (_currentHealth <= 0)
        {
            _exploded = true;
            RPC_PlayVFX();
            RPC_Explode();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_Explode()
    {
        int count = Runner.GetPhysicsScene().OverlapSphere(transform.position, _explosionRadius, _targets, _layerMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < count; i++)
        {
            var col = _targets[i];

            if (col.TryGetComponent<Player>(out var damageable))
            {
                damageable.RPC_TakeDamage(_damage);

                NetworkRigidbody3D[] rbs = col.GetComponents<NetworkRigidbody3D>();

                if (rbs != null)
                {
                    foreach (NetworkRigidbody3D rbchild in rbs)
                    {
                        rbchild.Rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, _explosionHeight, ForceMode.Impulse);
                    }
                }
            }
        }

        Runner.Despawn(Object);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_PlayVFX()
    {
        Runner.Spawn(_explosionVFX, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}