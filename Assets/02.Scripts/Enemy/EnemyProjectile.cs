using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyProjectile : PoolableMono
{
    [SerializeField] private float _power;
    [SerializeField] private float _duration  = 1.2f;
    private int _damage;

    private Transform _visualSprite;
    private Vector2 _moveDir;
    private Vector2 _targetDir;
    private Rigidbody2D _rigidbody;
    private float _angle;
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _visualSprite = transform.Find("visualSprite").transform;
    }
    private void Update()
    {
        _moveDir = _targetDir - (Vector2)transform.position;
        _angle = Mathf.Atan2(_moveDir.y, _moveDir.x) * Mathf.Rad2Deg;
        _visualSprite.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    }

    public virtual void StartShot(Vector2 targetDir,int damage)
    {
        _damage = damage;
        MoveToTarget(targetDir);
    }

    public virtual void MoveToTarget(Vector2 targetDir)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOJump(targetDir, _power, 1, _duration));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(DestroyProjectile);
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //IHittable hittable = collision.gameObject.GetComponent<IHittable>();
            //hittable?.GetHit(_damage, gameObject);
            Debug.Log("GetHit_Player");
        }
    }
    public override void Reset()
    {

    }

    public void DestroyProjectile()
    {
        PoolManager.Instance.Push(this);
    }
}
