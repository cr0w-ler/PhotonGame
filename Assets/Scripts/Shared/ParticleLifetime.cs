using Fusion;
using UnityEngine;

public class ParticleLifetime : NetworkBehaviour
{
    [SerializeField] float _lifetime = 3f;
    float _timer = 0;   

    public override void Spawned()
    {
        _timer = _lifetime;
    }

    public override void FixedUpdateNetwork()
    {
        _timer -= Runner.DeltaTime;

        if (_timer <= 0)
        {
            Runner.Despawn(Object);
        }
    }
}