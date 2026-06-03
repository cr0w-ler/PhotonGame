using System.Collections.Generic;
using UnityEngine;

public class LifeBarHandler : MonoBehaviour
{
    public static LifeBarHandler Instance { get; private set; }
    [SerializeField] private LifeBarItem lifeBarItemPrefab;
    private List<LifeBarItem> _lifeBarsList;
    
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _lifeBarsList = new List<LifeBarItem>();
    }

    public LifeBarItem AddLifeBar(LifeHandler owner)
    {
        if (owner == null) return null;

        LifeBarItem lifebar = Instantiate(lifeBarItemPrefab, transform);

        lifebar.SetOwner(owner);

        _lifeBarsList.Add(lifebar);

        owner.OnLeft += () =>
        {
            if (lifebar != null)
            {
                _lifeBarsList.Remove(lifebar);
                Destroy(lifebar.gameObject);
            }
        };
        //instanciamos una lifebar y seteamos el owner

        //lo agregamos a la lista

        //cuando el owner se va de la partida removemos la barra de vida y la destruimos

        //retornamos la nueva barra de vida

        return lifebar;
    }
    
    void LateUpdate()
    {
        foreach(var lifeBar in _lifeBarsList)
        {
            lifeBar.UpdatePosition();
        }
        //actualizo la posicion de mis lifebars
    }
}
