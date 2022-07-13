using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMove : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 5f;

    [SerializeField] private float _minPosX;
    [SerializeField] private float _maxPosX;

    [SerializeField] private float _deceleration;

    private float _currentSpeed;
    private float _currentDir = 0f;

    public void CameraMove(float dir)
    {
        if (dir == 0)
        {
            StartCoroutine(ReturnCoroutine());

        }
        else
        {
            StopAllCoroutines();
            if (dir > 0 && transform.position.x >= _maxPosX) return;
            if (dir < 0 && transform.position.x <= _minPosX) return;

            _currentDir = dir;

            _currentDir = Mathf.Clamp(_currentDir, -_maxSpeed, _maxSpeed);
            transform.Translate(Vector3.right * Time.deltaTime * _currentDir);
        }
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
            if (transform.position.x <= _minPosX || transform.position.x >= _maxPosX) yield break;

            transform.Translate(Vector3.right * _currentDir * Time.deltaTime );
            speed -= _deceleration * Time.deltaTime;

            _currentDir = speed * (isLeft ? -1f : 1f);
            yield return new WaitForEndOfFrame();
        }
    }

    public Tween MoveCameraPos(Vector3 pos, float duration)
    {
        return transform.DOMove(pos, duration);
    }

    public void SetCameraPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
