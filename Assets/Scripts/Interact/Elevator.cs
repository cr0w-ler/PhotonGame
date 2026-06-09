using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Elevator : NetworkBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] Transform[] _points;
    [SerializeField] NetworkRigidbody3D _rb;
    [Networked, OnChangedRender(nameof(OnActivatedChanged))]
    public NetworkBool IsActivated { get; private set; }

    public override void Spawned()
    {
        transform.position = _points[0].position;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetActivated(NetworkBool value)
    {
        IsActivated = value;
    }

    void OnActivatedChanged() { }

    public override void FixedUpdateNetwork()
    {
        Transform target = IsActivated ? _points[1] : _points[0];

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            _rb.Rigidbody.MovePosition(target.position);
            return;
        }

        MoveTo(target);
    }

    void MoveTo(Transform point)
    {
        Vector3 dir = (point.position - transform.position).normalized;
        _rb.Rigidbody.MovePosition(_rb.Rigidbody.position + dir * _speed * Runner.DeltaTime);
    }
}