using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private PoolListSO _initList = null;

    public enum GameState
    {
        Game,
        UI,
        Throwing,
        ThrowReady,
        Tutorial
    }

    public GameState gameState;

    private void Awake()
    {
        new PoolManager(transform);
        CreatePool();

        gameState = GameState.Game;
    }

    private void Start()
    {
        //if(DataManager.Inst.CurrentPlayer.isTutorial == false)
        //{
        //    gameState = GameState.Tutorial;
        //    GetComponent<TutorialManager>().StartTutorial();
        //    return;
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Inst.OnClickBackBtn();
        }
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


    public void QuitGame()
    {
        Application.Quit();
    }


}
