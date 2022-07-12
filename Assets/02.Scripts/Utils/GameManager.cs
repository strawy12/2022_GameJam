using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameManager : MonoSingleton<GameManager>
{

    public bool isClick;

    [SerializeField] private PoolListSO _initList = null;
    [SerializeField] private FollowCamera _followCamera;

    private MainCameraMove _mainCameraMove;

    public MainCameraMove MainCameraMove
    {
        get
        {
            if (_mainCameraMove == null)
                _mainCameraMove = Define.MainCam.GetComponent<MainCameraMove>();

            return _mainCameraMove;
        }
    }


    public FollowCamera FollowCamera => _followCamera;

    public enum GameState
    {
        Game,
        UI,
        Throwing,
        ThrowReady
    }

    public GameState gameState;

    private void Awake()
    {
        new PoolManager(transform);
        CreatePool();

        gameState = GameState.Game;
        _mainCameraMove = Define.MainCam.GetComponent<MainCameraMove>();
    }   

    private void CreatePool()
    {
        foreach (PoolingPair pair in _initList.list)
        {
            PoolManager.Instance.CreatePool(pair.prefab, pair.poolCnt);
        }
    }

    public void SetGold(int gold)
    {
        DataManager.Inst.CurrentPlayer.gold += gold;
        UIManager.Inst.GoldEvent();
    }

    public void StartFollow(Transform target)
    {
        _followCamera.SetTarget(target);
        _followCamera.StartFollow();
    }


    public void EndFollow()
    {
        _followCamera.EndFollow();
    }


}
