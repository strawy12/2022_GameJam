using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : AIAction
{
    public override void TakeAction()
    {
        _aIMovementData.direction = Vector2.zero;

        if (_aIActionData.attack == false)
        {
            _enemyAIBrain.Attack();
        }

        _enemyAIBrain.Move(_aIMovementData.direction);

    }
}
