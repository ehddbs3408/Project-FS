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

    [SerializeField]
    private float _invincibilityTime = 1f;
    private bool _isInvincibility = false;

    private int _currentHP = 0;
    public int CurrentHP => _currentHP;

    private bool _isDie = false;
    public bool IsDie => _isDie;

    public UnityEvent OnHitEvent = null;
    public UnityEvent OnDieEvent = null;

    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();

        _currentHP = _maxHP;
    }

    public void Hit()
    {
        if (_isInvincibility) return;

        _currentHP--;
        OnHitEvent?.Invoke();

        StartCoroutine(InvincibilityCoroutine());


        if(_currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDieEvent?.Invoke();

        DOTween.KillAll();
        _isDie = true;
        //Time.timeScale = 0f;
    }

    private IEnumerator InvincibilityCoroutine()
    {
        _isInvincibility = true;
        yield return new WaitForSeconds(0.2f);
        _sr.color = new Color(1, 1, 1, 0.5f);
        // 애니메이션 실행
        yield return new WaitForSeconds(_invincibilityTime - 0.2f);
        // 애니메이션 끄기
        _sr.color = new Color(1, 1, 1, 1f);
        _isInvincibility = false;
    }
}
