using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : AIAction
{
    public override void TakeAction()
    {
        _aIMovementData.direction.x = 0;

        if (_aIActionData.attack == false)
        {
            _enemyAIBrain.Attack();
        }

        _enemyAIBrain.Move(_aIMovementData.direction);

    }
}
