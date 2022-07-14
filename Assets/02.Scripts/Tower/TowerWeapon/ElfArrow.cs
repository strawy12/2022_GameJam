using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ElfArrow : PoolableMono
{
    private Vector2 _targetDir;
    private float _arrowForce;
    private int _damage;
    private Rigidbody2D _arrowRigidbody;
    private SpriteRenderer _spriteRenderer;
    private Sequence seq;
    private void Awake()
    {
        _arrowRigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector2 dir, float force, int damage)
    {
        _targetDir = dir;
        _arrowForce = force;
        _damage = damage;
        float seta = Mathf.Atan2(_targetDir.y, _targetDir.x);
        float rot = seta * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rot);

        ShootingArrow();
    }

    private void ShootingArrow()
    {
        _arrowRigidbody.AddForce(_targetDir * _arrowForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EventManager<Vector3>.TriggerEvent(Constant.TOWER_BOOM, transform.position);
            IHittable monsterHit = collision.GetComponent<IHittable>();
            monsterHit.GetHit(_damage, transform.gameObject);
            PoolManager.Instance.Push(this);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            EventManager<Vector3>.TriggerEvent(Constant.TOWER_BOOM, transform.position);
            _arrowRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            DeleteArrow();
        }
    }

    private void DeleteArrow()
    {
        seq = DOTween.Sequence();
        seq.Append(_spriteRenderer.DOFade(0, 1f));
        seq.AppendCallback(() => PoolManager.Instance.Push(this));
    }

    public override void Reset()
    {
        _arrowRigidbody.constraints = 0;
    }

}
