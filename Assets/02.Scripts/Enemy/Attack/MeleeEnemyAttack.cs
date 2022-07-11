using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttack : EnemyAttack
{

    public override void Attack(int damage)
    {
        if (!_waitBeforeNextAttack)
        {
            _enemyBrain.SetAttackState(true);

            //IHittable hitable = GetTarget().GetComponent<IHittable>();

            // hitable?.GetHit(damage: damage, damageDealer: gameObject);
            AttackFeedback?.Invoke();
            StartCoroutine(WaitBeforeAttackCoroutine());

            Debug.Log("¿Í±×ÀÛ!");
        }
    }
}
