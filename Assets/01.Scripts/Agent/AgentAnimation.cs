using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimation : MonoBehaviour
{
    protected Animator _animator;
    public Animator Animator
    {
        get
        {
            if(_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

    private readonly int _hashDie = Animator.StringToHash("Die");

    void Start()
    {
        if(_animator == null)
            _animator = GetComponent<Animator>();
    }

    public void DieAnimation()
    {
        _animator.Play(_hashDie);
    }
}
