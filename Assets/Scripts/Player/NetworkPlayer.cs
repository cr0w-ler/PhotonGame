using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(LocalInputs))]
public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }
    public LocalInputs LocalInputs { get; private set; }

    //se agrego un nickname item
    private NicknameItem _myNickname;
    
    [Networked]
    private NetworkString<_16> Nickname { get; set; }//Se agrego una variable networkeada para contener nuestro nickname en la red

    private ChangeDetector _changeDetector;//se agrego una variable de change detector

    public event Action OnLeft = delegate { };//crear un evento para cuando el jugador se va de la partida e invocarlo cuando el jugador abandona la partida

    public override void Spawned()
    {
        LocalInputs = GetComponent<LocalInputs>();

        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        _myNickname = NicknameHandler.Instance.AddNickname(this);

        if (Object.HasInputAuthority)
        {
            Local = this;
            LocalInputs.enabled = true;

            NetworkString<_16> loadedNick;
            
            if (PlayerPrefs.HasKey("Nickname"))
            {
                loadedNick = PlayerPrefs.GetString("Nickname");
            }
            else
            {
                loadedNick = $"Player {Runner.LocalPlayer.PlayerId}";
            }

            RPC_SetNickname(loadedNick);
        }
        else
        {
            LocalInputs.enabled = false;
            UpdateNickname();
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        // Avisamos a los suscriptores (NicknameHandler destruirá el NicknameItem)
        OnLeft();
    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(Nickname):

                    //Al detectar un cambio en el nickname tengo que updatearlo
                    UpdateNickname();
                    break;
            }
        }
    }

    //Crear metodo networkeado para setear nickname en la red
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetNickname(NetworkString<_16> newNickname)
    {
        Nickname = newNickname;
    }

    void UpdateNickname()
    {
        _myNickname.UpdateText(Nickname.Value);
        //Updateo mi nickname
    }
}