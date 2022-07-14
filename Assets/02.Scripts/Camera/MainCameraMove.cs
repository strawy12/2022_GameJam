using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMove : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 5f;

    [SerializeField] private float _deceleration;

    [SerializeField] private BoxCollider2D _limitCamCol;

    private float _currentSpeed;
    private float _currentDir = 0f;

    private Camera _mainCam;
    private void Awake()
    {
        _mainCam = Define.MainCam;
    }

    public void CameraMove(float dir)
    {
        if (dir == 0)
        {
            StartCoroutine(ReturnCoroutine());

        }
        else
        {
            StopAllCoroutines();

            if (LimitCheck(dir)) return;

            _currentDir = dir;

            _currentDir = Mathf.Clamp(_currentDir, -_maxSpeed, _maxSpeed);
            transform.Translate(Vector3.right * Time.deltaTime * _currentDir);
        }
    }

    private bool LimitCheck(float dir)
    {
        float posX = _mainCam.orthographicSize * _mainCam.aspect;

        if (dir > 0 && transform.position.x + posX >= _limitCamCol.bounds.max.x) return true;
        if (dir < 0 && transform.position.x - posX <= _limitCamCol.bounds.min.x) return true;


        return false;
    }

    private bool LimitCheck()
    {
        float posX = _mainCam.orthographicSize * _mainCam.aspect;

        if (transform.position.x + posX >= _limitCamCol.bounds.max.x) return true;
        if (transform.position.x - posX <= _limitCamCol.bounds.min.x) return true;


        return false;
    }

    public void StopImmediately()
    {
        StopAllCoroutines();
        _currentDir = 0f;
    }

    private IEnumerator ReturnCoroutine()
    {
        bool isLeft = _currentDir < 0;
        float speed = Mathf.Abs(_currentDir);

        while (speed > 0)
        {
            if (LimitCheck()) yield break;

            transform.Translate(Vector3.right * _currentDir * Time.deltaTime);
            speed -= _deceleration * Time.deltaTime;

            _currentDir = speed * (isLeft ? -1f : 1f);
            yield return new WaitForEndOfFrame();
        }
    }

    public Tween MoveCameraPos(Vector3 pos, float duration)
    {
        transform.DOKill();
        return transform.DOMove(pos, duration);
    }

    public void SetCameraPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
