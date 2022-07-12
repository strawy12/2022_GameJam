using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float effectSoundVolume;
    public float bgmSoundVolume;

    public bool isTutorial;

    public List<TowerData> towerDataList;
    public List<PlayerStatData> statDataList;
    public int currentRound;
    public int gold;
    public PlayerData(float soundVolume)
    {
        effectSoundVolume = soundVolume;
        bgmSoundVolume = soundVolume;
        isTutorial = false;
        towerDataList = new List<TowerData>();
        statDataList = new List<PlayerStatData>();
        currentRound = 0;
        gold = 10000000;

    }
}
