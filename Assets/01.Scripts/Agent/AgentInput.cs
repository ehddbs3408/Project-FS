using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentInput : MonoBehaviour
{
    private Agent _agent;

    public UnityEvent<Vector2> OnMovementAction = null;

    private void Start()
    {
        _agent = GetComponent<Agent>();
    }

    private void Update()
    {
        if (_agent.IsDie) return;

        OnMovementAction?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }
}
