using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameManager : MonoSingleton<GameManager>
{

    public bool isClick;

    [SerializeField] private PoolListSO _initList = null;
    [SerializeField] private MainCameraMove _mainCameraMove;

    public MainCameraMove MainCameraMove => _mainCameraMove;
    
    public enum GameState
    {
        Game,
        UI,
        Throwing
    }

    public GameState gameState;

    private void Awake()
    {
        new PoolManager(transform);
        CreatePool();

        gameState = GameState.Game;
    }

    private void CreatePool()
    {
        foreach (PoolingPair pair in _initList.list)
            PoolManager.Instance.CreatePool(pair.prefab, pair.poolCnt);
    }

    public void SetGold(int gold)
    {
        DataManager.Inst.CurrentPlayer.gold += gold;
        UIManager.Inst.GoldEvent();
    }
    

}
