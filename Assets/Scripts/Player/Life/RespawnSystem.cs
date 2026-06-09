using Fusion;
using System.Collections;
using UnityEngine;

public class RespawnSystem : NetworkBehaviour
{
    [SerializeField] HealthSystem _health;
    [SerializeField] byte _maxDeaths = 3;
    [SerializeField] float _delayToRespawn = 3f;

    byte _deathCount;

    public override void Spawned()
    {
        if (!HasStateAuthority) return;

        _health.OnDead += OnDied;
    }

    void OnDied()
    {
        if (!HasStateAuthority) return;

        _deathCount++;

        if (_deathCount >= _maxDeaths)
        {
            DisconnectPlayer();
            return;
        }

        StartCoroutine(RespawnCooldown());
    }

    IEnumerator RespawnCooldown()
    {
        yield return new WaitForSeconds(_delayToRespawn);

        _health.Resurrect();
    }

    void DisconnectPlayer()
    {
        GameManager.Instance.RPC_Defeat(Object.InputAuthority);

        //Runner.Disconnect(Object.InputAuthority);
        //Runner.Despawn(Object);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        _health.OnDead -= OnDied;
    }
}