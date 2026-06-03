using Fusion;
using UnityEngine;

public class CharacterRotator : NetworkBehaviour
{
    [Header("Rotation")]
    [SerializeField] protected float _speedRotation = 10f;
    [SerializeField] private Transform _mesh;
    public Transform Mesh => _mesh;

    public void RotateDefault(Vector3 dir)
    {
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        _mesh.rotation = Quaternion.Slerp(_mesh.rotation, targetRotation, _speedRotation * Runner.DeltaTime);
    }
}
