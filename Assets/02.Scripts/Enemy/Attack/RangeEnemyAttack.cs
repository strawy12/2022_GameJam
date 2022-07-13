using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyAttack : EnemyAttack
{
    [SerializeField] private Transform attackTrm;
    [SerializeField] private GameObject projectilePrefab;

    private Vector2 _moveDir;
    private int _damage;
    public override void Attack(int damage)
    {
        if(_waitBeforeNextAttack==false)
        {
            _enemyBrain.SetAttackState(true);
            AttackFeedback?.Invoke();

            _damage = damage;   
            StartCoroutine(WaitBeforeAttackCoroutine());
        }
    }
    public void SpawnArrow() 
    {
        EnemyProjectile projectile = PoolManager.Instance.Pop(projectilePrefab.gameObject.name) as EnemyProjectile;
        projectile.transform.position = attackTrm.position;
        projectile.StartShot(GetTarget().position, _damage);
    }
}
