using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOverCondition : AICondition
{
    [SerializeField]
    private float _time;

    public override bool IfCondition(AIState currentState, AIState nextState)
    {
        return _aiBrain.StateDuractionTime >= _time;
    }
}
