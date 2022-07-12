using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatData : ItemData
{
    public enum PlayerStat { DamageFactor, ThrowDelay, MaxHp }

    public PlayerStat statType;
    public float value;

    public PlayerStatData(PlayerStatData data) : base(data)
    {
        value = data.value;
        statType = data.statType;
    }
}

[CreateAssetMenu(menuName ="SO/DataList/StatDataList")]
public class PlayerStatDataSO : ScriptableObject
{
    public List<PlayerStatData> statDataList;

    public PlayerStatData this[int idx]
    {
        get => statDataList[idx];
        set => statDataList[idx] = value;
    }

    public int Count => statDataList.Count;
}
