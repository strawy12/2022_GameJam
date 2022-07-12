using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameManager : MonoSingleton<GameManager>
{

    public bool isClick;


    [SerializeField] private PoolListSO _initList = null;

    [SerializeField] private float _cameraSpeed = 5f;
    [SerializeField] private float _duration = 0f;

    [SerializeField] private Transform _cameraTransform;

    private float _currentDir = 0f;

    private void Awake()
    {
        new PoolManager(transform);
        CreatePool();
    }

    private void CreatePool()
    {
        foreach (PoolingPair pair in _initList.list)
            PoolManager.Instance.CreatePool(pair.prefab, pair.poolCnt);
    }


    public void CameraMove(float dir)
    {
        if (dir > 0)
        {
            _cameraTransform.Translate(Vector3.right * Time.deltaTime * _cameraSpeed);
            _currentDir = 1;
        }
        else if (dir < 0)
        {
            _cameraTransform.Translate(Vector3.left * Time.deltaTime * _cameraSpeed);
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
            _cameraTransform.Translate(Vector3.right * _currentDir * Time.deltaTime * moveSpeed);
            moveSpeed = Mathf.Lerp(0f, moveSpeed, time / _duration);
            yield return null;
            time -= Time.deltaTime;

        }
    }

}
