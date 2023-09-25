using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 10;

    [SerializeField]
    private SpriteRenderer _moveField;
    [SerializeField]
    private float _fieldOffset;

    public void Movement(Vector2 dir)
    {
        transform.position += new Vector3(dir.x, dir.y, 0) * _moveSpeed * Time.deltaTime;
        Vector2 move = transform.position;
        move.x = Mathf.Clamp(move.x, -(_moveField.size.x * 0.5f - _fieldOffset), _moveField.size.x * 0.5f - _fieldOffset);
        move.y = Mathf.Clamp(move.y, -(_moveField.size.y * 0.5f - _fieldOffset), _moveField.size.y * 0.5f - _fieldOffset);
        transform.position = move;
    }
}
