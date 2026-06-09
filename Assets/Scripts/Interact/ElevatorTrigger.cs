using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    [SerializeField] Elevator _elevator;
    [SerializeField] bool _activateOnEnter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            _elevator.RPC_SetActivated(_activateOnEnter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _elevator.RPC_SetActivated(!_activateOnEnter);
        }
    }
}