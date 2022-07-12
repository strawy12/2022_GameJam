using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDicision : AIDecision
{

    public float distance = 5f;

    public override bool MakeDecision()
    {
        float calc = Vector2.Distance(_enemyAIBrain.target.position, transform.position);
        if (calc <= distance)
        {
            if (_aIActionData.targetSpotted == false)
            {
                _aIActionData.targetSpotted = true;
            }
        }
        else
        {
            _aIActionData.targetSpotted = false;
        }
        return _aIActionData.targetSpotted;
    }
}
