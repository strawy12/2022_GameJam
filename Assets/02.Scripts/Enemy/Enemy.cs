using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Enemy : PoolableMono, IHittable, IKnockback
{
    [SerializeField] private EnemyDataSO _enemyData;
    public EnemyDataSO EnemyData => _enemyData;
    protected WaveController _waveController;
    protected bool _isDead = false;
    protected AgentMovement _agentMovement;
    protected EnemyAnimation _enemyAnimation;
    protected EnemyAttack _enemyAttack;
    protected BoxCollider2D _boxCollider;
    protected int _level = 1;

    protected EnemyAIBrain _enemyBrain;

    public int Health { get; private set; }
    public int Damage 
    { 
        get
        {
            int totalValue;
            totalValue = _enemyData.damage  + _enemyData.damage * (_level % 10) + _level/2;
            return totalValue;
        } 
    }
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

        Debug.Log(Health);

        if (Health <= 0)
        {
            Debug.Log("enemy ����");
            _isDead = true;
            _waveController.KillWaveMonster();
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
        _waveController = GameObject.Find("WaveController").GetComponent<WaveController>();
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
            //���⿡ �������� ������ �����ϴ� �ڵ�
            _enemyAttack.Attack(_enemyData.damage);
        }
    }

    public override void Reset()
    {
        _isActive = false;
        _isDead = false;
        _agentMovement.enabled = true;
        _boxCollider.enabled = true;
        _enemyAttack.Reset();
        _agentMovement.ResetKnockbackParam();
    }
    private void Start()
    {
        Health = Health = _enemyData.maxHealth;
    }

    public void Die()
    {
        PoolManager.Instance.Push(this);
    }
    public void SetEnemyStat(int level)
    {
        _level = level;
        Health = _enemyData.maxHealth + _enemyData.maxHealth * (_level % 10) / 2;
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
