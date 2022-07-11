using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : MonoBehaviour
{
    protected AIMovementData _aIMovementData;
    protected EnemyAIBrain _enemyAIBrain;
    protected AIActionData _aIActionData;
    protected virtual void Awake()
    {
        _aIMovementData = transform.GetComponentInParent<AIMovementData>();
        _enemyAIBrain = transform.GetComponentInParent<EnemyAIBrain>();
        _aIActionData = transform.GetComponentInParent<AIActionData>();
    }
    public abstract void TakeAction();
}
