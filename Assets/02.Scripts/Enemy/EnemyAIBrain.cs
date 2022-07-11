using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAIBrain : MonoBehaviour
{
    [field: SerializeField] public UnityEvent<Vector2> OnMovementKeyPress { get; set; }
    [field: SerializeField] public UnityEvent OnAttackButtonPress { get; set; }
    [SerializeField] protected AIState _currentState;

    public Transform target;
    
    public void ChangeState(AIState state)
    {
        _currentState = state;
    }
    public void Attack()
    {
        OnAttackButtonPress?.Invoke();
    }

    public void Move(Vector2 moveDirection)
    {
        OnMovementKeyPress?.Invoke(moveDirection);
    }
    protected virtual void Update()
    {
        if (target == null)
        {
            OnMovementKeyPress?.Invoke(Vector2.zero);
        }
        else
        {
            _currentState.UpdateState();
        }
    }
}
