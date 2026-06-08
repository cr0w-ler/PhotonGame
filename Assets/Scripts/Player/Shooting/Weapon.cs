using Fusion;
using System;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] float _fireRate = 0.5f;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _shotSpawnTransform;
    [Networked] public bool _readyToFire { get; private set; }
    public event Action OnShot = delegate { };
    [Networked] TickTimer _tickTimer { get; set; }

    public override void Spawned()
    {
        _tickTimer = TickTimer.CreateFromSeconds(Runner, _fireRate);
    }

    public override void FixedUpdateNetwork()
    {
        _readyToFire = _tickTimer.Expired(Runner);
        Debug.Log("Pos " +_shotSpawnTransform.position);
    }

    public void Fire()
    {
        if (!HasStateAuthority) return;

        SpawnBullet();

        OnShot?.Invoke();

        ResetTimer();
    }

    void SpawnBullet()
    {
        Runner.Spawn(_bulletPrefab, _shotSpawnTransform.position, _shotSpawnTransform.rotation, Object.InputAuthority);
    }

    void ResetTimer()
    {
        _tickTimer = TickTimer.CreateFromSeconds(Runner, _fireRate);
    }
}