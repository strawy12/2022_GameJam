using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerStat
{
    public int towerNum;
    public int damage = 1;
    public int level;
    public bool isLock;

    public TowerStat(int num)
    {
        towerNum = num;
        damage = 1;
        level = 0;
        isLock = true;
    }
}
