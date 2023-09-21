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

        // ������ �߻�
        Debug.Log("������ �߻�");
        LaserBullet laserBullet = GameManager.Instance.ResourceManager_.Instantiate("LaserBullet").GetComponent<LaserBullet>();
        // �������� ���̸� �����ֱ�
        laserBullet.transform.position = this.transform.position; // �ַ��� �Ƹ��� laserBullet�� Null
        laserBullet.transform.rotation = this.transform.rotation;
        laserBullet.DestroyAction -= () => GameManager.Instance.ResourceManager_.Destroy(this.gameObject);
        laserBullet.DestroyAction += () => GameManager.Instance.ResourceManager_.Destroy(this.gameObject);
    }
}
