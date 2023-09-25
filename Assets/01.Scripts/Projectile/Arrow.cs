using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    public override void Init(Vector2 direction)
    {
        _direction = (Vector3)direction - this.transform.position;
        _direction.Normalize();
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        _attackCorutine = StartCoroutine(AttackCorutine());
    }
}
