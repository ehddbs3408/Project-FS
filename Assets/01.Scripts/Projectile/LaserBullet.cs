using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    private Laser _parentLaser = null;

    private bool _isHit = false;

    private Coroutine _destroyCoroutine = null;

    private void OnEnable()
    {
        StartCoroutine(Destroy());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_isHit == false)
            {
                _isHit = true;

                // 데미지 처리하고
                // 이펙트 정도?
                Agent agent = collision.GetComponentInParent<Agent>();
                if (agent != null)
                {
                    agent.Hit();
                }
            }
        }
    }

    public void SetParentLaser(Laser laser)
    {
        _parentLaser = laser;
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);

        _parentLaser.Destroy();
        _parentLaser = null;
        _isHit = false;
        GameManager.Instance.ResourceManager_.Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if(_destroyCoroutine != null)
        {
            StopCoroutine(_destroyCoroutine);
            _destroyCoroutine = null;
        }
    }
}
