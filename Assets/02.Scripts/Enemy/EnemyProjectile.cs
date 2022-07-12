using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : PoolableMono
{
    [SerializeField] private float speed;
    private int _damage;

    private bool isActive = false;
    private Vector2 _moveDir = Vector2.zero;
    private void Update()
    {
        if (isActive)
        {
            transform.Translate(_moveDir * Time.deltaTime * speed);
        }
    }

    public virtual void StartShot(Vector2 dir,int damage)
    {
        _damage = damage;
        _moveDir = dir;
        isActive = true;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //IHittable hittable = collision.gameObject.GetComponent<IHittable>();
            //hittable?.GetHit(_damage, gameObject);
            Debug.Log("GetHit_Player");
            DestroyProjectile();
        }
    }
    public override void Reset()
    {

    }

    public void DestroyProjectile()
    {
        isActive = false;
        PoolManager.Instance.Push(this);
    }
}
