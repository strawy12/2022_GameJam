using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ElfArrow : PoolableMono
{
    private Vector2 _targetDir;
    private float _arrowForce;

    private Rigidbody2D _arrowRigidbody;
    private SpriteRenderer _spriteRenderer;
    private Sequence seq;
    private void Awake()
    {
        _arrowRigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector2 dir, float force)
    {
        _targetDir = dir;
        _arrowForce = force;

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
            IHittable monsterHit = collision.GetComponent<IHittable>();
            monsterHit.GetHit(2, transform.gameObject);
            PoolManager.Instance.Push(this);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
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
