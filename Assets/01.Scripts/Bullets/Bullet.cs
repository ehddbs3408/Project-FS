using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    protected Vector3 _direction;

    protected bool _moveable = false;

    [SerializeField]
    protected float _delay = 1f;
    [SerializeField]
    protected float _moveSpeed = 5f;

    public UnityEvent OnAttackEvent = null;

    protected Coroutine _attackCorutine;

    public virtual void Init(Vector2 direction)
    {
        _direction = (Vector3)direction - this.transform.position;
        _direction.Normalize();

        _attackCorutine = StartCoroutine(AttackCorutine());
    }

    protected virtual IEnumerator AttackCorutine()
    {
        yield return new WaitForSeconds(_delay);

        OnAttackEvent?.Invoke(); // 애니메이션 실행

        _moveable = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 데미지 처리하고
            // 이펙트 정도?
            Agent agent = collision.GetComponentInParent<Agent>();
            if(agent != null)
            {
                agent.Hit();
            }

            Destroy();
        }
    }

    private void Update()
    {
        if (_moveable)
        {
            transform.position += _direction * _moveSpeed * Time.deltaTime;

            Vector3 pos = Define.MainCam.WorldToViewportPoint(transform.position);
            if(pos.x < 0f || pos.x > 1f || pos.y < 0f || pos.y > 1f)
            {
                Destroy();
            }
        }
    }

    public void Destroy()
    {
        if (_attackCorutine != null)
        {
            StopCoroutine(_attackCorutine);
            _attackCorutine = null;
        }

        BulletManager.Instance.RemoveBullet(this);
        GameManager.Instance.ResourceManager_.Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        BulletManager.Instance.RemoveBullet(this);
        if (_attackCorutine != null)
        {
            StopCoroutine(_attackCorutine);
            _attackCorutine = null;
        }
    }
}
