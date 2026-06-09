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
       
        //agrego el nickname a la lista

        //subscribo al evento del networkplayer un metodo que remueve el nickname y destruye el objeto.
        //retornar el nickname creado

        var newNickname = Instantiate(_nicknameItemPrefab, transform)
            .SetOwner(owner);

        _nicknames.Add(newNickname);

        owner.OnLeft += () =>
        {
            _nicknames.Remove(newNickname);
            Destroy(newNickname.gameObject);
        };

        return newNickname;
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