using Fusion;
using System.Collections;
using UnityEngine;

public class WeaponView : NetworkBehaviour
{
    [SerializeField] Weapon _weapon;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] ParticleSystem _shootVFX;
    [SerializeField] Light _light;
    [SerializeField] float _delay = 0.1f;
    Coroutine _coroutine;

    public override void Spawned()
    {
        if (_weapon)
        {
            _weapon.OnShot += PlayVFX;
        }
    }

    public void PlayVFX()
    {
        if (_coroutine == null)
        {
            StartCoroutine(LightCoroutine());
        }

        RPC_PlayShootVFX();
        RPC_PlayShootSFX();
    }

    IEnumerator LightCoroutine()
    {
        _light.enabled = true;
        yield return new WaitForSeconds(_delay);
        _light.enabled = false;
        _coroutine = null;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    void RPC_PlayShootVFX()
    {
        _shootVFX.Play();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    void RPC_PlayShootSFX()
    {
        _audioSource.Play();
    }
}