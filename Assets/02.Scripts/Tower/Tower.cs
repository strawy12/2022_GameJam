using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public abstract class Tower : PoolableMono
{
    public enum ETower { Stone, Fire, PieceMaker, Elf, Pyramid }
    [SerializeField] private ETower _towerType;
    [SerializeField] protected GameObject _effectPrefab;
    [SerializeField] protected ParticleSystem _throwEffect;
    [SerializeField] protected PoolParticle _destroyParticle;
    [SerializeField] protected Vector2 _groundCheckOverlapOffestVec;  
    [SerializeField] protected Vector2 _groundCheckOverlapSize;
    [SerializeField] protected LayerMask _isWhatGround;
    
    protected TowerData _towerData;

    protected Rigidbody2D _rigidbody;
    protected Collider2D _collider;
    protected SpriteRenderer _spriteRenderer;

    protected bool _isStop = false;

    public TowerData Data => _towerData;
    public Rigidbody2D Rigid => _rigidbody;
    public Collider2D Collider => _collider;
    public Action OnEndThrow;
    public UnityEvent OnGroundTower;

    protected Sequence seq;
    protected virtual void Awake()
    {
        AwakeInit();
    }

    private void AwakeInit()
    {
        _spriteRenderer ??= transform.Find("VisualSprite").GetComponent<SpriteRenderer>();
        _collider ??= transform.Find("VisualSprite").GetComponent<Collider2D>();
        _rigidbody ??= GetComponent<Rigidbody2D>();

        _towerData ??= DataManager.Inst.CurrentPlayer.GetTowerData((int)_towerType);
    }

    protected virtual void Start()
    {
        StartInit();
    }

    private void StartInit()
    {
        _isStop = false;
        _rigidbody.constraints = 0;
        Collider.enabled = false;
        _spriteRenderer.enabled = true;
        _spriteRenderer.DOFade(1, 0.01f);
        Rigid.isKinematic = true;
        _spriteRenderer.transform.localRotation = Quaternion.identity;
    }

    public override void Reset()
    {
        AwakeInit();
        StartInit();
    }
    private void Update()
    {
        if (!_isStop)
        {
            GroundOverlap();
            float angle = Mathf.Atan2(_rigidbody.velocity.y, _rigidbody.velocity.x) * Mathf.Rad2Deg - 90f;
            ChangeAngle(angle);
        }
    }

    internal void ChangeAngle(float angle)
    {
        _spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public virtual void DestroyTower()
    {
        CancelInvoke();
        if (_destroyParticle != null)
        {
            PoolParticle particle = PoolManager.Instance.Pop(_destroyParticle.gameObject.name) as PoolParticle;
            particle.OnEnableParticle(transform.position);
        }
        Invoke("PushTower", 3f);
    }
    public void PushTower()
    {
        StopAllCoroutines();
        PoolManager.Instance.Push(this);
    }
   
    public abstract void UseSkill();

    public virtual void StartThrow()
    {
        transform.SetParent(null);
        _throwEffect.Play();
        OnThrowTower();
    }

    protected virtual void OnThrowTower()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OnTriggerEnemy(collision);
        }
    }

    public void GroundOverlap()
    {
        Vector2 origin = new Vector2(transform.position.x + _groundCheckOverlapOffestVec.x, transform.position.y + _groundCheckOverlapOffestVec.y);
        Collider2D col = Physics2D.OverlapBox(origin, _groundCheckOverlapSize, _spriteRenderer.transform.rotation.z, _isWhatGround);
        if (col != null)
        {
            _isStop = true;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            _rigidbody.isKinematic = true;
            _throwEffect.Stop();
            switch (_towerData.towerType)
            {
                case ETowerType.PassiveType:
                case ETowerType.FixingType:
                    UseSkill();
                    break;
                case ETowerType.ActiveType:
                    DestroyTower();
                    break;
                default:
                    break;
            }
            Define.MainCam.DOShakePosition(0.5f, 1.5f, 10);
            GameManager.Inst.gameState = GameManager.GameState.Game;
            OnEndThrow?.Invoke();
            SpawnEffect();
        }
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

    protected virtual void SpawnEffect() // ¹Ù´Ú¿¡ ºÙÀÌ Ä¥¶§ ÀÌÆåÆ®
    {
        Vector2 rayPos = transform.position;
        rayPos.y = 10f;
        var hit = Physics2D.Raycast(rayPos, Vector2.down, 999f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            Effect effect = PoolManager.Instance.Pop(_effectPrefab.name) as Effect;
            effect.transform.SetPositionAndRotation(hit.point, Quaternion.identity);
            effect.StartAnim();
            EventManager<Vector3>.TriggerEvent(Constant.TOWER_BOOM, transform.position);
        }
        ShakeObject(hit.point);
    }

    protected void ShakeObject(Vector2 hitPoint)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitPoint, new Vector2(20f, 3f), 0f, LayerMask.GetMask("Enemy"));

        foreach (var hit in hits)
        {
            IShake shakeObj = hit.GetComponent<IShake>();

            if (shakeObj != null)
            {
                shakeObj.StartShake();
            }
        }
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + _groundCheckOverlapOffestVec.x, transform.position.y + _groundCheckOverlapOffestVec.y), _groundCheckOverlapSize);
    }
#endif
}
