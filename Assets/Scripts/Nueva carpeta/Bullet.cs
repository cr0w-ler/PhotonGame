using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime;

    private TickTimer _lifeTimer;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            _lifeTimer = TickTimer.CreateFromSeconds(Runner, _lifeTime);
        }
    }

    public override void FixedUpdateNetwork()
    {
        Move();

        if (_lifeTimer.ExpiredOrNotRunning(Runner))
        {
            if (HasStateAuthority)
            {
                Runner.Despawn(Object);
            }
        }
    }

    void Move()
    {
        transform.position += transform.forward * _speed * Runner.DeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.TryGetComponent(out Player player))
        {
            player.RPC_TakeDamage(_damage);
        }

        Runner.Despawn(Object);
    }
}