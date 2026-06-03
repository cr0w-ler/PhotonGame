using System;
using Fusion;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [SerializeField] private Transform _shotSpawnTransform;

    public event Action OnShot = delegate { };

    public void Fire()
    {
        if (!HasStateAuthority) return;

        SpawnBullet();

        RayBullet();

        OnShot();
    }

    void SpawnBullet()
    {
        Runner.Spawn(_bulletPrefab, _shotSpawnTransform.position, _shotSpawnTransform.rotation);
    }

    void RayBullet()
    {
        if (Runner == null)
        {
            Debug.LogError("Runner is NULL!");
            return;
        }

        if (Object == null)
        {
            Debug.LogError("NetworkObject is NULL!");
            return;
        }

        Debug.DrawLine(transform.position, transform.position + transform.forward * 2, Color.magenta, 2);

        Runner.LagCompensation.Raycast(origin: transform.position, 
                                        direction: transform.forward, 
                                        length: 100, 
                                        player: Object.InputAuthority, 
                                        hit: out var hitInfo);


        if (hitInfo.Hitbox == null) return;

        if (!hitInfo.Hitbox.transform.root.TryGetComponent(out LifeHandler player)) return;
        
        player.TakeDamage(25);
    }
}
