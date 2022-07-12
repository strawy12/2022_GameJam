using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMove : MonoBehaviour
{
    [SerializeField] private float _cameraSpeed = 5f;
    [SerializeField] private float _duration = 10f;

    private Camera _mainCam;
    private float _currentDir = 0f;
    private void Awake()
    {
        _mainCam = Define.MainCam;
    }

    public void CameraMove(float dir)
    {
        if (dir > 0)
        {
            _mainCam.transform.Translate(Vector3.right * Time.deltaTime * _cameraSpeed);
            _currentDir = 1;
        }
        else if (dir < 0)
        {
            _mainCam.transform.Translate(Vector3.left * Time.deltaTime * _cameraSpeed);
            _currentDir = -1;
        }
        else
        {
            StartCoroutine(ReturnCoroutine());
        }
    }

    private IEnumerator ReturnCoroutine()
    {
        float time = _duration;
        float moveSpeed = _cameraSpeed;

        while (time > 0)
        {
            _mainCam.transform.Translate(Vector3.right * _currentDir * Time.deltaTime * moveSpeed);
            moveSpeed = Mathf.Lerp(0f, moveSpeed, time / _duration);
            yield return null;
            time -= Time.deltaTime;

        }
    }

    public Tween MoveCameraPos(Vector3 pos, float duration)
    {
        return _mainCam.transform.DOMove(pos, duration);
    }

    public void SetCameraPos(Vector3 pos)
    {
        _mainCam.transform.position = pos;
    }
}
