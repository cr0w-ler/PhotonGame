using System.Collections;
using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField] ParticleSystem _vfx;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Light _light; 
    [SerializeField] float _delay = 0.1f;
    Coroutine _coroutine;
    public void PlayVFX()
    {
        if (_coroutine == null)
        {
            StartCoroutine(LightCoroutine());
        }

        _vfx.Play();
        _audioSource.Play();
    }

    private IEnumerator LightCoroutine()
    {
        _light.enabled = true;
        yield return new WaitForSeconds(_delay);
        _light.enabled = false;
        _coroutine = null;
    }
}