using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimation : AgentAnimation
{
    private readonly int _moveHash = Animator.StringToHash("Move");
    private readonly int _idleHash = Animator.StringToHash("Idle");

    public void Move()
    {
        if(_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        _animator.Play(_moveHash);
    }
}
