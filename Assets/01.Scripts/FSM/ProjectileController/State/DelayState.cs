using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayState : AIState
{
    [SerializeField]
    private float _delayTime = 3f;

    private List<AIState> _stateList = new List<AIState>();

    public override void OnStateEnter()
    {
        if(_stateList.Count <= 0)
        {
            _aiBrain.GetComponentsInChildren<AIState>(_stateList);
            _stateList.Remove(this);
        }
    }

    public override void TakeAAction()
    {
        if(_aiBrain.StateDuractionTime >= _delayTime)
        {
            AIState state = _stateList[0];
            _stateList.RemoveAt(0);
            _aiBrain.ChangeState(state);
        }
    }
}
