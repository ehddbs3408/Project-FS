using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Arrow
{
    private BulletAnimation _bulletAnimation;

    private void Start()
    {
        if(_bulletAnimation == null)
            _bulletAnimation = GetComponentInChildren<BulletAnimation>();
    }

    protected override IEnumerator AttackCorutine()
    {
        OnAttackEvent?.Invoke();

        if (_bulletAnimation == null)
            _bulletAnimation = GetComponentInChildren<BulletAnimation>();

        yield return new WaitUntil(() =>
            _bulletAnimation.Animator.GetCurrentAnimatorStateInfo(0).IsName("Move")
            && _bulletAnimation.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        // 레이저 발사
        Debug.Log("레이저 발사");
    }
}
