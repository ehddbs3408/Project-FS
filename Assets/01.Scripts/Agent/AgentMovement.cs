using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    private float _h = 0;
    private float _v = 0;

    [SerializeField]
    private float _moveSpeed = 10;

    public void Movement(Vector2 dir)
    {
        transform.position += new Vector3(dir.x, dir.y, 0) * _moveSpeed * Time.deltaTime;
    }
}
