using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : AIAction
{
    public override void TakeAction()
    {
        _aIMovementData.direction.x = 0;
        _enemyAIBrain.Move(_aIMovementData.direction);
    }
}
