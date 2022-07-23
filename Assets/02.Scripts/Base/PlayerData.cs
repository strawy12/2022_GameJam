using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public bool isTutorial;

    public List<TowerData> towerDataList;
    public List<PlayerStatData> statDataList;

    public int[] equipTowerIdArr;
    public int waveIndex;
    public int waveLevel;
    public long gold;
    public bool versionCheck;

    public PlayerData()
    {

        isTutorial = false;
        towerDataList = new List<TowerData>();
        statDataList = new List<PlayerStatData>();
        equipTowerIdArr = Enumerable.Repeat(-1, 5).ToArray();
        waveIndex = 0;
        waveLevel = 1;
        gold = 0;
        versionCheck = true;

    }

    public float GetStat(PlayerStatData.EPlayerStat stat)
    {
        return statDataList.Find(x => x.statType == stat).value;
    }

    public bool EqualArrValue(int id)
    {
       foreach(var towerId in equipTowerIdArr)
        {
            if (towerId == id)
                return true;
        }

        return false;
    }

    public TowerData GetTowerData(int towerNum)
    {
        return towerDataList.Find(tower => tower.itemNum == towerNum);
    }
}
