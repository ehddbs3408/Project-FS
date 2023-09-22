using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private int _maxHP = 3;
    public int MaxHP => _maxHP;

    private int _currentHP = 0;
    public int CurrentHP => _currentHP;

    private void Awake()
    {
        _currentHP = _maxHP;
    }

    public void Hit()
    {
        _currentHP--;
        Debug.Log("Hit");

        if(_currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Time.timeScale = 0f;
        // UI ¶ç¿ì±â

        Debug.Log("Game Over");
    }
}
