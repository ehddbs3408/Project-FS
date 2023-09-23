using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayState : AIState
{
    [SerializeField]
    private float _delayTime = 3f;

    public override void TakeAAction()
    {
        if(_aiBrain.StateDuractionTime >= _delayTime)
        {
            // 여기서 ChangeState로 처리해도 되고 밖에서 해도 되고
        }
    }
}
