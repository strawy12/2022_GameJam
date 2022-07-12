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

            _moveDir = GetTarget().position - attackTrm.position;
            _moveDir.Normalize();
            _damage = damage;
   
            StartCoroutine(WaitBeforeAttackCoroutine());
        }
    }
    public void SpawnArrow() 
    {
        EnemyProjectile projectile = PoolManager.Instance.Pop(projectilePrefab.gameObject.name) as EnemyProjectile;
        projectile.StartShot(_moveDir, _damage);
        projectile.transform.position = attackTrm.position;
    }
}
