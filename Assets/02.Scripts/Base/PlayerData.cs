using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public bool isTutorial;

    public List<TowerData> towerDataList;
    public List<PlayerStatData> statDataList;
    public int waveIndex;
    public int waveLevel;
    public long gold;
    public bool versionCheck;

    public PlayerData()
    {

        isTutorial = false;
        towerDataList = new List<TowerData>();
        statDataList = new List<PlayerStatData>();
        waveIndex = 0;
        waveLevel = 0;
        gold = 0;
        versionCheck = true;

    }

    public float GetStat(PlayerStatData.EPlayerStat stat)
    {
        return statDataList.Find(x => x.statType == stat).value;
    }

    public TowerData GetTowerData(int towerNum)
    {
        return towerDataList.Find(tower => tower.itemNum == towerNum);
    }
}
