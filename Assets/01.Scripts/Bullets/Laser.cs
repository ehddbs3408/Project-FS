using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Arrow
{
    [SerializeField]
    private bool _isPreview = false;

    private GameObject _laserPreview;

    private BulletAnimation _bulletAnimation;

    private void Start()
    {
        if(_bulletAnimation == null)
            _bulletAnimation = GetComponentInChildren<BulletAnimation>();
    }

    protected override IEnumerator AttackCorutine()
    {
        OnAttackEvent?.Invoke();

        if(_isPreview)
        {
            _laserPreview = GameManager.Instance.ResourceManager_.Instantiate("LaserPreview");
            _laserPreview.transform.position = this.transform.position;
            _laserPreview.transform.rotation = this.transform.rotation;
        }

        if (_bulletAnimation == null)
            _bulletAnimation = GetComponentInChildren<BulletAnimation>();

        yield return new WaitUntil(() =>
            _bulletAnimation.Animator.GetCurrentAnimatorStateInfo(0).IsName("Move")
            && _bulletAnimation.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        if(_isPreview && _laserPreview != null)
        {
            GameManager.Instance.ResourceManager_.Destroy(_laserPreview);
            _laserPreview = null;
        }
        // 레이저 발사
        Debug.Log("레이저 발사");
        LaserBullet laserBullet = GameManager.Instance.ResourceManager_.Instantiate("LaserBullet").GetComponent<LaserBullet>();
        // 레이저의 길이를 정해주기
        laserBullet.transform.position = this.transform.position;
        laserBullet.transform.rotation = this.transform.rotation;
        laserBullet.SetParentLaser(this);
    }
}
