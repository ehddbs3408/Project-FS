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
            // ���⼭ ChangeState�� ó���ص� �ǰ� �ۿ��� �ص� �ǰ�
        }
    }
}
