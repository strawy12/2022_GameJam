using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfArrow : PoolableMono
{
    private Vector2 _targetDir;
    private float _arrowForce;

    private Rigidbody2D _arrowRigidbody;

    public void Init(Vector2 dir, float force)
    {
        if (_arrowRigidbody == null)
            _arrowRigidbody = GetComponent<Rigidbody2D>();

        _targetDir = dir;
        _arrowForce = force;

        ShootingArrow();
    }

    private void ShootingArrow()
    {
        _arrowRigidbody.AddForce(_targetDir * _arrowForce);
    }

    public override void Reset()
    {
        if (_arrowRigidbody == null)
            _arrowRigidbody = GetComponent<Rigidbody2D>();

        Debug.Log(11);
    }

}
