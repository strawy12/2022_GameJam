using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyAttack : MonoBehaviour
{
    protected EnemyAIBrain _enemyBrain;
    protected EnemyAttack _enemy;

    public float attackDelay = 1;

    protected bool _waitBeforeNextAttack;

    public bool WaitingForNextAttack => _waitBeforeNextAttack;

    public UnityEvent AttackFeedback;
    protected virtual void Awake()
    {
        _enemy = GetComponent<EnemyAttack>();
        _enemyBrain = GetComponent<EnemyAIBrain>();
    }

    protected IEnumerator WaitBeforeAttackCoroutine()
    {
        _waitBeforeNextAttack = true;
        yield return new WaitForSeconds(attackDelay);
        _waitBeforeNextAttack = false;
        EndOfAttackAnimation();
    }

    public void Reset()
    {
        StopAllCoroutines();
        _waitBeforeNextAttack = false;
       // StartCoroutine(WaitBeforeAttackCoroutine());
    }

    public void EndOfAttackAnimation()
    {
        _enemyBrain.ActionData.attack = false;
    }
    public virtual void HitMotionPlay()
    {

    }
    public Transform GetTarget()
    {
        return _enemyBrain.target;
    }
    public abstract void Attack(int damage);
}
