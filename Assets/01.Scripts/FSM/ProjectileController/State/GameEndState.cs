using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEndState : AIState
{
    public UnityEvent OnGameEndEvent = null;

    public override void OnStateEnter()
    {
        OnGameEndEvent?.Invoke();
    }

    public override void OnStateLeave()
    {

    }

    public override void TakeAAction()
    {

    }
}
