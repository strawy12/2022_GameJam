using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAIBrain : MonoBehaviour
{
    public bool isPeaceMaker = false;

    [field: SerializeField] public UnityEvent<Vector2> OnMovementKeyPress { get; set; }
    [field: SerializeField] public UnityEvent OnAttackButtonPress { get; set; }
    [SerializeField] protected AIState _currentState;
    protected AIActionData _actionData;
    public AIActionData ActionData { get => _actionData; }
    public Transform target;
    protected virtual void Awake()
    {
        _actionData = transform.Find("AI").GetComponent<AIActionData>();
    }

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
    public void SetAttackState(bool state)
    {
        _actionData.attack = state;
    }
    protected virtual void Update()
    {
        if (target == null || isPeaceMaker)
        {
            OnMovementKeyPress?.Invoke(Vector2.zero);
        }
        else
        {
            _currentState.UpdateState();
        }
    }
}
