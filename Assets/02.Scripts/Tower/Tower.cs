using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : PoolableMono
{
    protected TowerData _towerData;
    public TowerData Data => _towerData;

    protected bool isStop = false;
    protected Transform baseTrm;
    protected Rigidbody2D _rigidbody;
    public Rigidbody2D Rigid => _rigidbody;

    private Collider2D _collider;
    public Collider2D Collider => _collider;

    private SpriteRenderer _spriteRenderer;

    private ParticleSystem _particle;

    private bool _isThrow;

    protected virtual void Awake()
    {
        baseTrm = transform.Find("baseTransform");
        _particle = transform.Find("TowerShootParticle").GetComponent<ParticleSystem>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Init()
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody2D>();
        if (_collider == null)
            _collider = GetComponent<Collider2D>();

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (_towerData == null)
            _towerData = DataManager.Inst.CurrentPlayer.towerDataList.Find(tower => tower.prefabName.Equals(name));
     }
    public override void Reset()
    {
        Init();
        isStop = false;
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
        _particle.Play();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            isStop = true;
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
            EventManager.TriggerEvent(Constant.END_THROW_TOWER);

        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !isStop)
        {
            IHittable hittable = collision.GetComponent<IHittable>();
            hittable?.GetHit(_towerData.damage, gameObject);
        }
    }
}
