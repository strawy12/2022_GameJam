using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Tower : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    public Rigidbody2D Rigid => _rigidBody;

    private Collider2D _collider;
    public Collider2D Collider => _collider;

    public void Init()
    {
        if (_rigidBody == null)
            _rigidBody = GetComponent<Rigidbody2D>();
        if (_collider == null)
            _collider = GetComponent<Collider2D>();
    }
}
