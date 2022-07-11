using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction : AIAction
{
    public override void TakeAction()
    {
        Vector3 dir = _enemyAIBrain.target.position - transform.position;
        dir.Normalize();
        _aIMovementData.direction = new Vector2(dir.x, 0);
        _enemyAIBrain.Move(_aIMovementData.direction);
    }
}
