using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndState : AIState
{
    public override void OnStateEnter()
    {
        Debug.Log("Game End");
    }

    public override void OnStateLeave()
    {

    }

    public override void TakeAAction()
    {

    }
}
