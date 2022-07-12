using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Tower : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    public Rigidbody2D Rigid => _rigidBody;

    private Collider2D _collider;
    public Collider2D Collider => _collider;

    private SpriteRenderer _spriteRenderer;

    private bool _isThrow;

    [SerializeField] private LayerMask _targetLayer;

    public void Init()
    {
        if (_rigidBody == null)
            _rigidBody = GetComponent<Rigidbody2D>();
        if (_collider == null)
            _collider = GetComponent<Collider2D>();

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void StartThrow()
    {
        _isThrow = true;
    }

    public void Update()
    {
        if (_isThrow == false) return;
        float angle = Mathf.Atan2(_rigidBody.velocity.y, _rigidBody.velocity.x) * Mathf.Rad2Deg - 90f;
        ChangeAngle(angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1<<collision.gameObject.layer)& _targetLayer) != 0)
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.isKinematic = true;
            _isThrow = false;
            EventManager.TriggerEvent(Constant.END_THROW_TOWER);
        }
    }

    internal void ChangeAngle(float angle)
    {
        _spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
