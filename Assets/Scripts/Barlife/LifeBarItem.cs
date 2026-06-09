using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarItem : MonoBehaviour
{
    private Transform _owner;
    private const float HEAD_OFFSET = 1.5F;
    [SerializeField] private Image _myLifeImage;
    [SerializeField] private float _lerpRate = 0.3f;

    public LifeBarItem SetOwner(HealthSystem owner)
    {
        _owner = owner.transform;
        return this;
    }

    public void UpdateLife(float val)
    {
        StopAllCoroutines();
        StartCoroutine(UpdateLifeOnTime(val));
    }

    IEnumerator UpdateLifeOnTime(float val)
    {
        float startValue = _myLifeImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < _lerpRate)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / _lerpRate;
            _myLifeImage.fillAmount = Mathf.Lerp(startValue, val, t);

            yield return null;
        }

        _myLifeImage.fillAmount = val;
    }
    
    public void UpdatePosition()
    {
        transform.position = _owner.transform.position + Vector3.up * HEAD_OFFSET;
        //movemos la barra de vida arriba de nuestro dueño
    }
}