using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : PoolableMono
{
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

    public Action OnEndThrow;

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

        _towerData ??= DataManager.Inst.CurrentPlayer.towerDataList.Find(tower => tower.prefabName.Equals(name));
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

    public virtual void UseSkill()
    {

    }
    public virtual void StartThrow()
    {
        _isThrow = true;
        transform.SetParent(null);
        _particle.Play();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isStop = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            _isThrow = false;
            _particle.Stop();
            if (_towerData.towerType == ETowerType.PassiveType)
            {
                UseSkill();
            }
            if (_towerData.towerType == ETowerType.ActiveType && GameManager.Inst.isClick)
            {
                //UseSkill();
                //GameManager.Inst.isClick = false;
            }
            if (_towerData.towerType == ETowerType.FixingType)
            {
                UseSkill();
            }

            OnEndThrow?.Invoke();
            GameManager.Inst.EndFollow();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !_isStop)
        {
            IHittable hittable = collision.GetComponent<IHittable>();
            hittable?.GetHit(_towerData.damage, gameObject);
            IKnockback knockback = collision.GetComponent<IKnockback>();
            knockback?.Knockback(Vector2.one, _towerData.knockbackPower,1f);
        }
    }
}
