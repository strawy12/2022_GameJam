using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowedTower : MonoBehaviour
{
    [SerializeField] private float _reloadDelay;   
    [SerializeField] private float _maxForce;
    [SerializeField] private float _forceOffset;
    [SerializeField] private Tower _towerPref;
    [SerializeField] private ThrowLine _throwLine = null;

    private float _force;
    private Camera _mainCam;

    private Vector2 _throwDir;
    private bool _isPressed = false;
    private Vector2 _startMousePos;

    private Tower _currentTower;
    private bool _isReloading;

    private void Awake()
    {
        _mainCam = Define.MainCam;

        _currentTower = Instantiate(_towerPref, transform.position, Quaternion.identity);
        _currentTower.Init();
        _currentTower.Rigid.isKinematic = true;
        _currentTower.gameObject.SetActive(true);

        _isReloading = false;
    }

    private void Update()
    {
        if (_isReloading) return;
        if (_isPressed)
        {
            Vector2 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            _throwDir = mousePos - (Vector2)transform.position;
            _throwDir.Normalize();

            _force = Vector2.Distance(_startMousePos, mousePos) * _forceOffset;

            _force = Mathf.Clamp(_force, 0f, _maxForce);

            float angle = Mathf.Atan2(-_throwDir.y, -_throwDir.x) * Mathf.Rad2Deg - 90f;
            _currentTower.ChangeAngle(angle);
            _throwLine.DrawGuideLine(_currentTower.Rigid, transform.position, -_throwDir * _force, 300);
        }


    }

    private void OnMouseDown()
    {
        if (_isReloading) return;

        _isPressed = true;
        _currentTower.Rigid.isKinematic = true;
        _startMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (_isReloading) return;

        EventManager<Transform>.TriggerEvent(Constant.START_THROW_TOWER, _currentTower.transform);
        _currentTower.StartThrow();
        _isPressed = false;
        _currentTower.Collider.enabled = true;
        _currentTower.Rigid.isKinematic = false;
        _currentTower.Rigid.velocity = (-_throwDir * _force);
        _force = 0f;
        _currentTower = null;
        _isReloading = true;
        _throwLine.ClearLine();


        StartCoroutine(Release());
    }

    private IEnumerator Release()
    {
        yield return new WaitForSeconds(_reloadDelay);

        // 풀매니저 사용
        _currentTower = PoolManager.Instance.Pop(_towerPref.gameObject.name) as Tower;
        _currentTower.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        _currentTower.Init();
        _currentTower.Collider.enabled = false;
        _currentTower.Rigid.isKinematic = true;
        _currentTower.gameObject.SetActive(true);

        
        _isReloading = false;
    }
}
