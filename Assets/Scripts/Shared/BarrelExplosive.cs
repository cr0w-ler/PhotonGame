using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class BarrelExplosive : NetworkBehaviour
{
    [SerializeField] GameObject _explosionVFX;
    [SerializeField] HealthSystem _health;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionHeight;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _damage;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] int _capacityArray = 50;
    [Networked] private NetworkBool _exploded { get; set; }
    Collider[] _targets;

    public override void Spawned()
    {
        _targets = new Collider[_capacityArray];

        if (_health)
            _health.OnDead += RPC_Explode;
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (_health)
            _health.OnDead -= RPC_Explode;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_Explode()
    {
        if (_exploded) return;

        _exploded = true;
        Runner.Spawn(_explosionVFX, transform.position, Quaternion.identity);

        int count = Runner.GetPhysicsScene().OverlapSphere(transform.position, _explosionRadius, _targets, _layerMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < count; i++)
        {
            var col = _targets[i];

            if (col.TryGetComponent<HealthSystem>(out var damageable))
            {
                damageable.RPC_TakeDamage(_damage);
            }

            foreach (var rbchild in col.GetComponents<NetworkRigidbody3D>())
            {
                rbchild.Rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, _explosionHeight, ForceMode.Impulse);
            }

        }

        Runner.Despawn(Object);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}