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
    #endregion

    protected bool isStop = false;
    protected Transform baseTrm;
    protected Rigidbody2D _rigidbody;

    protected virtual void Awake()
    {
        baseTrm = transform.Find("baseTransform");
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void Reset()
    {
        isStop = false;
        _rigidbody.constraints = 0;
    }

    public virtual void DestroyTower()
    {
        return;
        PoolManager.Instance.Push(this);
    }

    public virtual void StartThrowed()
    {

    }

    public virtual void UseSkill()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            isStop = true;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            
            if (_towerStatData.towerType == ETowerType.PassiveType)
            {
                UseSkill();
            }
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !isStop)
        {
            
        }
    }
}
