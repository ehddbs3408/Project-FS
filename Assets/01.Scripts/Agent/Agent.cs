using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Agent : MonoBehaviour
{
    [SerializeField]

    private int _maxHP = 3;
    public int MaxHP => _maxHP;

    private int _currentHP = 0;
    public int CurrentHP => _currentHP;

    public UnityEvent OnHitEvent = null;
    public UnityEvent OnDieEvent = null;

    private void Awake()
    {
        _currentHP = _maxHP;
    }

    public void Hit()
    {
        _currentHP--;
        OnHitEvent?.Invoke();


        if(_currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDieEvent?.Invoke();

        DOTween.KillAll();
        Time.timeScale = 0f;
    }
}
