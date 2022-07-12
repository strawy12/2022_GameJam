using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : PoolableMono
{
    #region TowerData
    [SerializeField] protected TowerStatSO _towerStatData;

    public int Damage
    {
        get
        {
            return _towerStatData.damage;
        }
    }

    public float Weight
    {
        get
        {
            return _towerStatData.weight;
        }
    }

    public ETowerType TowerType 
    { 
        get
        {
            return _towerStatData.towerType;
        }
    }

    protected TowerData _towerData;
    public TowerData Data => _towerData;
    #endregion

    protected bool isStop = false;
    protected Transform baseTrm;
    protected Rigidbody2D _rigidbody;
    public Rigidbody2D Rigid => _rigidbody;

    private Collider2D _collider;
    public Collider2D Collider => _collider;

    private SpriteRenderer _spriteRenderer;

    private bool _isThrow;

    protected virtual void Awake()
    {
        baseTrm = transform.Find("baseTransform");
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
    }
    public override void Reset()
    {
        isStop = false;
        _isThrow = false;
        _rigidbody.constraints = 0;
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

    public virtual void StartThrowed()
    {

    }

    internal void ChangeAngle(float angle)
    {
        _spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public virtual void UseSkill()
    {
        
    }
    public void StartThrow()
    {
        _isThrow = true;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            isStop = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            _isThrow = false;
            if (_towerStatData.towerType == ETowerType.PassiveType)
            {
                UseSkill();
            }
            if (_towerStatData.towerType == ETowerType.ActiveType && GameManager.Inst.isClick)
            {
                     //UseSkill();
                //GameManager.Inst.isClick = false;
            }
            if (_towerStatData.towerType == ETowerType.FixingType)
            {
                UseSkill();
            }
            EventManager.TriggerEvent(Constant.END_THROW_TOWER);

        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !isStop)
        {
            
        }
    }
}
