using Fusion;
using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] int _maxScore = 9999;
    [Networked] public int _currentScore {  get; private set; }

    public event Action<int> OnScoreChanged;

    private void Start()
    {
       OnScoreChanged?.Invoke(_currentScore);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void AddScore(int score)
    {
        if (score <= 0) return;

        _currentScore = Mathf.Min(_currentScore + score, _maxScore);
        OnScoreChanged?.Invoke(_currentScore);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void SubtractScore(int score)
    {
        if (score <= 0) return;

        _currentScore = Mathf.Max(_currentScore -  score, 0);
        OnScoreChanged?.Invoke(_currentScore);
    }
}