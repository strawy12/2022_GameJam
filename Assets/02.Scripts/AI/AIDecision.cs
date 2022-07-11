using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecision : MonoBehaviour
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

    public abstract bool MakeDecision();//transition을 일으킬것인지 아닌지를 결정해서 bool로 반환
}
