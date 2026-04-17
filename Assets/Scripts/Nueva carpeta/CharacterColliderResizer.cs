using Fusion;
using UnityEngine;

public class CharacterColliderResizer : NetworkBehaviour
{
    [SerializeField] CapsuleCollider _col;
    [SerializeField] Transform _mesh;

    public void SetSize(float size, Vector3 center)
    {
        _col.height = size;
        _col.center = center;
    }
}