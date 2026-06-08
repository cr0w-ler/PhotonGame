using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] TMPro.TextMeshProUGUI _scoreText;

    private void Start()
    {
        _wallet.OnScoreChanged += UpdateScoreText;
        UpdateScoreText(_wallet._currentScore);
    }

    private void UpdateScoreText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }
    private void OnDestroy()
    {
        _wallet.OnScoreChanged -= UpdateScoreText;
    }
}
