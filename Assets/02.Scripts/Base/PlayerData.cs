using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float effectSoundVolume;
    public float bgmSoundVolume;

    public bool isTutorial;

    public List<TowerStat> towerStatDataList;
    public int currentRound;
    public int gold;
    public PlayerData(float soundVolume)
    {
        effectSoundVolume = soundVolume;
        bgmSoundVolume = soundVolume;
        isTutorial = false;
        towerStatDataList = new List<TowerStat>();
        currentRound = 0;
        gold = 0;

    }
}
