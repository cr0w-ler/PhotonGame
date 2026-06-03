using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private Transform _targetTransform;
    [SerializeField] private Vector3 _offset;

    public void SetTarget(Player player)
    {
        _targetTransform = player.transform;
    }

    private void LateUpdate()
    {
        if (!_targetTransform) return;

        transform.position = _targetTransform.position + _offset;
        transform.LookAt(_targetTransform);
    }
}