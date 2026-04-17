using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Ragdoll : NetworkBehaviour
{
    [SerializeField] NetworkRigidbody3D[] _ragdollRBs;
    [SerializeField] NetworkMecanimAnimator _animatorNetwork;
    [SerializeField] Animator _animator;

    public void DisableRagdoll()
    {
        foreach (var rb in _ragdollRBs)
        {
            rb.Rigidbody.isKinematic = true;
        }

        _animator.enabled = true;
        _animatorNetwork.enabled = true;
    }

    public void ActivateRagdoll()
    {
        _animator.enabled = false;
        _animatorNetwork.enabled = false;

        foreach (var rb in _ragdollRBs)
        {
            rb.Rigidbody.isKinematic = false;
        }

    }
}