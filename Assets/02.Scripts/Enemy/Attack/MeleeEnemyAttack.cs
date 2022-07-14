using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttack : EnemyAttack
{

    public override void Attack(int damage)
    {
        if (!gameObject.activeSelf) return;

        if (!_waitBeforeNextAttack)
        {
            _enemyBrain.SetAttackState(true);

            IHittable hitable = GetTarget().GetComponentInParent<IHittable>();

            hitable?.GetHit(damage: damage, damageDealer: gameObject);
            AttackFeedback?.Invoke();

            StartCoroutine(WaitBeforeAttackCoroutine());
        }
    }
}
