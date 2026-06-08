using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int _scorePoints = 50;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Wallet player))
        {
            player.AddScore(_scorePoints);
            Destroy(gameObject);
        }
    }
}