using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowedTower : MonoBehaviour
{
    [SerializeField] private float _maxForce;
    [SerializeField] private float _forceOffset;
    [SerializeField] private Rigidbody2D _rigidbody;
    

    private float _force;
    private Camera _mainCam;
    [SerializeField] private ThrowLine _throwLine = null;

    private Vector2 _throwDir;
    private bool _isPressed = false;
    private Vector2 _startMousePos;

    private void Awake()
    {
        _mainCam = Define.MainCam;
        _rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (_isPressed)
        {
            Vector2 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            _throwDir = mousePos - (Vector2)transform.position;
            _throwDir.Normalize();

            _force = Vector2.Distance(_startMousePos, mousePos) * _forceOffset;

            _force = Mathf.Clamp(_force, 0f, _maxForce);

            _throwLine.DrawGuideLine(_rigidbody, transform.position, -_throwDir * _force, 300);
        }


    }

    private void OnMouseDown()
    {
        _isPressed = true;
        _rigidbody.isKinematic = true;
        _startMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        _isPressed = false;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = (-_throwDir * _force);
        _force = 0f;
    }

}
