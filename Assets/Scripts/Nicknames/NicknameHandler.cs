using System.Collections.Generic;
using UnityEngine;

public class NicknameHandler : MonoBehaviour
{
    public static NicknameHandler Instance { get; private set; }

    [SerializeField] private NicknameItem _nicknameItemPrefab;

    private List<NicknameItem> _nicknames;
    
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _nicknames = new List<NicknameItem>();
    }

    public NicknameItem AddNickname(NetworkPlayer owner)
    {
        //instancio un nickname como hijo y seteo su dueño
        if (owner == null) return null;

        NicknameItem nickname = Instantiate(_nicknameItemPrefab, transform);

        nickname.SetOwner(owner);

        _nicknames.Add(nickname);

        owner.OnDespawned += () =>
        {
            if (nickname != null)
            {
                _nicknames.Remove(nickname);
                Destroy(nickname.gameObject);
            }
        };

        //agrego el nickname a la lista

        //subscribo al evento del networkplayer un metodo que remueve el nickname y destruye el objeto.

        return nickname;//retornar el nickname creado
    }
    
    void LateUpdate()
    {
        foreach(var nick in _nicknames)
        {
            nick.UpdatePosition();
        }

        //actualizo las posiciones de todos los nicknames
    }
}
