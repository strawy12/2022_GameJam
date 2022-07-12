using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Enemy : PoolableMono, IHittable, IKnockback
{
    [SerializeField] private EnemyDataSO _enemyData;
    public EnemyDataSO EnemyData => _enemyData;

    protected bool _isDead = false;
    protected AgentMovement _agentMovement;
    protected EnemyAnimation _enemyAnimation;
    protected EnemyAttack _enemyAttack;
    protected BoxCollider2D _boxCollider;

    //죽었을때 처리할 것과
    //액티브 상태를 관리할 애가 필요

    protected EnemyAIBrain _enemyBrain;

    #region 인터페이스 구현부
    public int Health { get; private set; }
    [field: SerializeField] public UnityEvent OnDie { get; set; }
    [field: SerializeField] public UnityEvent OnGetHit { get; set; }

    public bool IsEnemy => true;

    public Vector3 HitPoint { get; private set; }
    public virtual void GetHit(int damage, GameObject damageDealer)
    {
        if (_isDead) return;
     
        Health -= damage;
        HitPoint = damageDealer.transform.position;
        OnGetHit?.Invoke();
        if (Health <= 0)
        {
            _isDead = true;
            _agentMovement.StopImmediatelly();
            _agentMovement.enabled = false;
            OnDie?.Invoke();
            _enemyAttack.Reset();
        }
    }
    #endregion

    [SerializeField]
    private bool _isActive = false;
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _agentMovement = GetComponent<AgentMovement>();
        _enemyAnimation = transform.GetComponentInChildren<EnemyAnimation>();
        _enemyAttack = GetComponent<EnemyAttack>();
        _enemyBrain = GetComponent<EnemyAIBrain>();
        _enemyAttack.attackDelay = _enemyData.attackDelay;
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Ground");
        }
    }
    public virtual void PerformAttack()
    {
        if (!_isDead && _isActive)
        {
            //여기에 실제적인 공격을 수행하는 코드
            _enemyAttack.Attack(_enemyData.damage);
        }
    }

    public override void Reset()
    {
        _isActive = false;
        Health = _enemyData.maxHealth;
        _isDead = false;
        _agentMovement.enabled = true;
        _boxCollider.enabled = true;
        _enemyAttack.Reset();
        _agentMovement.ResetKnockbackParam();
    }
    private void Start()
    {
        Health = _enemyData.maxHealth;
    }
    public void Die()
    {
        PoolManager.Instance.Push(this);
    }

    public void Knockback(Vector2 dir, float power, float duration)
    {
        if (!_isDead && !_isActive)
        {
            if (power > _enemyData.knockbackRegist)
            {
                _agentMovement.KnockBack(dir, power, duration);
            }
        }
    }
}
