using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject _winImage;
    [SerializeField] private GameObject _loseImage;

    private List<PlayerRef> _players;//PlayerRef sirve como identificacion de cada cliente conectado

    private void Awake()
    {
        Instance = this;

        _players = new List<PlayerRef>();
    }

    public void AddToList(Player player)
    {
        var playerRef = player.Object.StateAuthority;
        //Consigo el objecto con state autority
        //lo agrego a _players si no lo contiene.

        if (!_players.Contains(playerRef))
        {
            _players.Add(playerRef);
        }
    }

    void RemoveFromList(PlayerRef player)
    {
        if (_players.Contains(player))
        {
            _players.Remove(player);
        }
    }

    [Rpc]
    public void RPC_Defeat(PlayerRef player)
    {
        //si el player que perdio es el local llamo al metodo local de derrota
        if (player == Runner.LocalPlayer)
            ShowDefeatPanel();

        //Llamo al metodo para removerme de la lista
        _players.Remove(player);

        //si queda un solo jugadore en _players y tengo state authority llamo a la victoria
        if (_players.Count == 1 && HasStateAuthority)
            RPC_Win(_players[0]);
    }

    //[RpcTarget] El llamado del RPC va a ir dirigido a ese jugador
    [Rpc]
    void RPC_Win([RpcTarget] PlayerRef player)
    {
        ShowWinPanel();
    }

    void ShowWinPanel()
    {
        _winImage.SetActive(true);
    }

    void ShowDefeatPanel()
    {
        _loseImage.SetActive(true);
    }
}