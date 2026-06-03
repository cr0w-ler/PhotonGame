using Fusion;
using UnityEngine;

public class CharacterMeshSelector : NetworkBehaviour
{
    [SerializeField] private GameObject[] _meshes;
    [SerializeField] private NetworkMecanimAnimator[] _mecanimAnims;
    public NetworkMecanimAnimator[] MecanimAnims => _mecanimAnims;

    public void SelectMesh(int index)
    {
        for (int i = 0; i < _meshes.Length; i++)
        {
            _meshes[i].SetActive(i == index);
        }
    }
}
