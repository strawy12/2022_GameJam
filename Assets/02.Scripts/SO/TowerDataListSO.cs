using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETowerType
{
    ActiveType,
    PassiveType,
    FixingType
}

[System.Serializable]
public class TowerData : ItemData
{
    [Header("타워 정보")]
    public string prefabName;
    public float damage;
    public float weight;
    public bool isLock;
    public float knockbackPower;
    public ETowerType towerType;
        
    public TowerData(TowerData data) : base(data)
    {
        prefabName = data.prefabName;
        damage = data.damage;
        weight = data.weight;
        isLock = data.isLock;
        towerType = data.towerType;
        knockbackPower = data.knockbackPower;
    }
}

[CreateAssetMenu(menuName = "SO/DataList/TowerDataList")]
public class TowerDataListSO : ScriptableObject
{
    public List<TowerData> towerDataList;

    public TowerData this[int idx]
    {
        get => towerDataList[idx];
        set => towerDataList[idx] = value;
    }

    public int Count => towerDataList.Count;

}
