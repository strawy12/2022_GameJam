using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Constant;

public class Enemy : PoolableMono, IHittable, IKnockback, IShake
{
    [SerializeField] private EnemyDataSO _enemyData;
    [SerializeField] private PoolParticle _dieParticle;
    [SerializeField] private Effect _dieEffect;
    public EnemyDataSO EnemyData => _enemyData;
    protected WaveController _waveController;
    protected bool _isDead = false;
    protected AgentMovement _agentMovement;
    protected EnemyAnimation _enemyAnimation;
    protected EnemyAttack _enemyAttack;
    protected BoxCollider2D _boxCollider;
    protected int _level = 1;
    protected bool _isStiff = false;
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
    [field: SerializeField] public UnityEvent OnShake { get; set; }

    public bool IsEnemy => true;

    public Vector3 HitPoint { get; private set; }

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
    public virtual void GetHit(int damage, GameObject damageDealer)
    {
        if (_isDead || _isStiff) return;

        float playerCritical = DataManager.Inst.CurrentPlayer.GetStat(PlayerStatData.EPlayerStat.Critical);

        bool isCritical = IsCritical(playerCritical > 100 ? 100 : playerCritical);

        if (isCritical)
        {
            float criticalFactor = playerCritical > 100f ? playerCritical - 100f : 1.5f;

            damage = (int)(damage * criticalFactor);
        }

        Health -= damage;

        SpawnDamagePopup(damage, isCritical);

        StartCoroutine(StiffCoroutine());
        HitPoint = damageDealer.transform.position;
        OnGetHit?.Invoke();

        if (Health <= 0)
        {
            _isDead = true;
            _waveController.KillWaveMonster(this);
            _agentMovement.StopImmediatelly();
            _agentMovement.enabled = false;
            GameManager.Inst.SetGold(_enemyData.dropCoin * _level / 2);
            PoolParticle particle = PoolManager.Instance.Pop(_dieParticle.gameObject.name) as PoolParticle;
            particle.SetParticleVelocity(new Vector2(transform.position.x,transform.position.y + 1f),damageDealer.transform.position);
            Effect effect = PoolManager.Instance.Pop(_dieEffect.gameObject.name) as Effect;
            effect.transform.position = transform.position; 
            effect.StartAnim();
            OnDie?.Invoke();
            _enemyAttack.Reset();
        }
    }

    protected void SpawnDamagePopup(int damage,bool isCritical)
    {
        DamagePopup popup = PoolManager.Instance.Pop("DamagePopup") as DamagePopup;

        popup.Setup(damage, transform.position, isCritical);
    }

    private bool IsCritical(float playerCritical)
    {
        float critical = UnityEngine.Random.value;
        bool isCritical = false;

        if (critical <= (playerCritical / CRITICAL_MAX_PERCENT))
        {
            isCritical = true;
        }
        return isCritical;
    }


    public virtual void PerformAttack()
    {
        if (!_isDead && _isActive)
        {
            _enemyAttack.Attack(_enemyData.damage);
        }
    }
    public IEnumerator StiffCoroutine()
    {
        _isStiff = true;
        yield return new WaitForSeconds(0.5f);
        _isStiff = false;
    }
    public override void Reset()
    {
        _isActive = true;
        _isDead = false;
        _isStiff = false;
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

    public void StartShake()
    {
        OnShake?.Invoke();
    }

    public void WaveDeath()
    {
        _isDead = true;
        _agentMovement.StopImmediatelly();
        _agentMovement.enabled = false;
        _enemyAttack.Reset();
        Die();
    }
}
