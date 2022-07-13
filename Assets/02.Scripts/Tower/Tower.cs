using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Tower : PoolableMono
{
    public enum ETower { Stone, Fire, PieceMaker, Elf, Pyramid }
    [SerializeField] private ETower _towerType;
    [SerializeField] protected GameObject _effectPrefab; 
    [SerializeField] private ParticleSystem _throwEffect;
    [SerializeField] private PoolParticle _destroyParticle;
    protected TowerData _towerData;

    protected Transform _baseTrm;

    protected Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private SpriteRenderer _spriteRenderer;

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
        _spriteRenderer ??= transform.Find("VisualSprite").GetComponent<SpriteRenderer>();
        _collider ??= transform.Find("VisualSprite").GetComponent<Collider2D>();
        _rigidbody ??= GetComponent<Rigidbody2D>();

        _towerData ??= DataManager.Inst.CurrentPlayer.GetTowerData((int)_towerType);
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
        _spriteRenderer.transform.rotation = Quaternion.identity;
    }
    private void Update()
    {
        if (_isThrow == false) return;

        float angle = Mathf.Atan2(_rigidbody.velocity.y, _rigidbody.velocity.x) * Mathf.Rad2Deg - 90f;
        ChangeAngle(angle);
    }

    public virtual void DestroyTower()
    {
        if(_destroyParticle !=null)
        {
            PoolParticle particle = PoolManager.Instance.Pop(_destroyParticle.gameObject.name) as PoolParticle;
            particle.OnEnableParticle(transform.position);
        }
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
        _throwEffect.Play();
        OnThrowTower();
    }
    protected virtual void OnThrowTower()
    {

    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !_isStop)
        {
            _isStop = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            _throwEffect.Stop();
            _isThrow = false;
            switch (_towerData.towerType)
            {
                case ETowerType.PassiveType:
                    UseSkill();
                    break;
                case ETowerType.ActiveType:
                    _isGround = true;
                    FadeTower(0f);
                    break;
                case ETowerType.FixingType:
                    UseSkill();
                    break;
                default:
                    break;
            }
            OnEndThrow?.Invoke();
            GameManager.Inst.EndFollow();

            SpawnEffect();
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
        seq.Append(_spriteRenderer.DOFade(0, 0.1f));
        seq.AppendCallback(DestroyTower);
    }
    protected virtual void OnTriggerEnemy(Collider2D collision)
    {
        if (!_isStop)
        {
            IHittable hittable = collision.GetComponent<IHittable>();
            float damage = (_towerData.damage * DataManager.Inst.CurrentPlayer.GetStat(PlayerStatData.EPlayerStat.DamageFactor));
           
            hittable?.GetHit((int)damage, gameObject);
            IKnockback knockback = collision.GetComponent<IKnockback>();
            knockback?.Knockback(Vector2.one, _towerData.knockbackPower, 1f);
        }
    }
    

    protected abstract void SpawnEffect();
    protected void ShakeObject(Vector2 hitPoint)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitPoint, new Vector2(20f, 3f), 0f, LayerMask.GetMask("Enemy"));

        foreach(var hit in hits)
        {
            IShake shakeObj = hit.GetComponent<IShake>();

            if(shakeObj != null)
            {
                shakeObj.StartShake();
            }
        }
    }
}
