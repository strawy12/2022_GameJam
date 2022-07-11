using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Enemy : PoolableMono
{
    [SerializeField] private EnemyDataSO _enemyData;
    public EnemyDataSO EnemyData => _enemyData;
    
    public int Health { get; private set; }
    [field: SerializeField] public UnityEvent OnDie { get; set; }
    [field: SerializeField] public UnityEvent OnGetHit { get; set; }

    protected bool _isDead = false;
    protected EnemyAnimation _enemyAnimation;
    protected EnemyAIBrain _enemyBrain;
    protected EnemyAttack _enemyAttack;
    private bool _isActive = false;

    private void Awake()
    {
        _enemyAttack = GetComponent<EnemyAttack>();
        _enemyBrain = GetComponent<EnemyAIBrain>();
        _enemyAnimation = transform.Find("visualSprite").GetComponent<EnemyAnimation>();
    }

    public override void Reset()
    {
        _isActive = false;
        Health = _enemyData.maxHealth;
        _isDead = false;
        _enemyAttack.Reset();
    }
    public virtual void PerformAttack()
    {
        if (!_isDead && _isActive)
        {
            //여기에 실제적인 공격을 수행하는 코드
            _enemyAttack.Attack(_enemyData.damage);
        }
    }

    public void Die()
    {
        PoolManager.Instance.Push(this);
    }
}
