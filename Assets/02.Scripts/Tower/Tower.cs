using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Constant;

public abstract class Tower : PoolableMono
{
    [SerializeField] private int _towerNum;


    protected TowerData _towerData;

    protected Transform _baseTrm;

    protected Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _particle;

    protected bool _isStop = false;
    private bool _isThrow;

    public TowerData Data => _towerData;
    public Rigidbody2D Rigid => _rigidbody;
    public Collider2D Collider => _collider;
    private bool _isGround;
    public Action OnEndThrow;
    protected Sequence seq;
    protected virtual void Awake()
    {
        StartInit();
    }

    private void StartInit()
    {
        _baseTrm ??= transform.Find("BaseTransform");
        _particle ??= transform.Find("TowerShootParticle").GetComponent<ParticleSystem>();
        _spriteRenderer ??= transform.Find("VisualSprite").GetComponent<SpriteRenderer>();
        _collider ??= transform.Find("VisualSprite").GetComponent<Collider2D>();
        _rigidbody ??= GetComponent<Rigidbody2D>();

        _towerData ??= DataManager.Inst.CurrentPlayer.GetTowerData(_towerNum);
    }

    private void Start()
    {
    }

    public override void Reset()
    {
        StartInit();
        _isStop = false;
        _isThrow = false;
        _rigidbody.constraints = 0;
        Collider.enabled = false;
        _spriteRenderer.DOFade(1, 0.01f);
        Rigid.isKinematic = true;
    }
    private void Update()
    {
        if (_isThrow == false) return;

        float angle = Mathf.Atan2(_rigidbody.velocity.y, _rigidbody.velocity.x) * Mathf.Rad2Deg - 90f;
        ChangeAngle(angle);
    }

    public virtual void DestroyTower()
    {
        PoolManager.Instance.Push(this);
    }

    internal void ChangeAngle(float angle)
    {
        _spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public abstract void UseSkill();

    public virtual void StartThrow()
    {
        _isThrow = true;
        transform.SetParent(null);
        _particle.Play();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !_isStop)
        {
            _isStop = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            _isThrow = false;
            _particle.Stop();
            switch (_towerData.towerType)
            {
                case ETowerType.PassiveType:
                    UseSkill();
                    break;
                case ETowerType.ActiveType:
                    _isGround = true;
                    break;
                case ETowerType.FixingType:
                    UseSkill();
                    break;
                default:
                    break;
            }
            OnEndThrow?.Invoke();
            GameManager.Inst.EndFollow();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OnTriggerEnemy(collision);
        }
    }

    public void FadeTower(float delay)
    {
        seq = DOTween.Sequence();
        seq.AppendInterval(delay);
        seq.Append(_spriteRenderer.DOFade(0, 1f));
        seq.AppendCallback(DestroyTower);
    }

protected virtual void OnTriggerEnemy(Collider2D collision)
{
    if(!_isStop)
    {
        IHittable hittable = collision.GetComponent<IHittable>();
        float damage = (_towerData.damage * DataManager.Inst.CurrentPlayer.GetStat(PlayerStatData.EPlayerStat.DamageFactor));
        if (IsCritical())
            damage *= CRITICAL_DAMAGE_FACTOR;
        hittable?.GetHit((int)damage, gameObject);
        IKnockback knockback = collision.GetComponent<IKnockback>();
        knockback?.Knockback(Vector2.one, _towerData.knockbackPower, 1f);
    }
}
    private bool IsCritical()
    {
        float critical = UnityEngine.Random.value;
        bool isCritical = false;

        if (critical <= (DataManager.Inst.CurrentPlayer.GetStat(PlayerStatData.EPlayerStat.Critical) / CRITICAL_MAX_PERCENT))
        {
            isCritical = true;
        }
        return isCritical;
    }
}
