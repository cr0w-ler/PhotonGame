using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class MyMainMenu : NetworkBehaviour
{
    [SerializeField] GameObject _menuUI;
    [SerializeField] Button _connectButton;
    [SerializeField] Button _exitButton;

    private void Start()
    {
        _connectButton.onClick.AddListener(Connect);
        _exitButton.onClick.AddListener(Exit);
    }

    private async void Connect()
    {

    }

    private void Exit()
    {
        Application.Quit();
    }
}
